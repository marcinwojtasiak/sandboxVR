using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour
{
    public PlayerRepresentation playerRep;

    public Transform baseTransform;

    public Transform rightFoot;
    public Transform leftFoot;

    [Range(0f, .2f)] public float footBaseOffset;
    [Range(0f, 1f)] public float ikStrength;


    private float rightFootSurfaceHight;
    private float leftFootSurfaceHight;
    private Quaternion rightFootRotation;
    private Quaternion leftFootRotation;

    private float rayCastOriginOffsetHeight = 0.4f;

    private Animator animator;
    private bool leftFootIKActive;
    private bool rightFootIKActive;

    private int mask;

    public void Start()
    {
        animator = GetComponent<Animator>();
        mask = 1 << 9; //player mask
        mask = ~mask;
    }

    public void Update() //ewentualnie dodac sprawdzanie czy stopa nie jest gdzies za wysoko i wylaczyc wtedy IK
    {
        CheckSurface();
    }

    public void OnAnimatorIK()
    {
        //upper body and head
        animator.SetLookAtWeight(1);
        animator.SetLookAtPosition(playerRep.localHeadset.position + playerRep.localHeadset.forward);

        //right hand position
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikStrength);
        animator.SetIKPosition(AvatarIKGoal.RightHand, playerRep.localRightHand.transform.position);

        //right rotation
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikStrength);
        animator.SetIKRotation(AvatarIKGoal.RightHand, playerRep.localRightHand.transform.rotation);

        //left hand position
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikStrength);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, playerRep.localLeftHand.transform.position);

        //left rotation
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikStrength);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, playerRep.localLeftHand.transform.rotation);

        if (rightFootIKActive)
        {
            //right foot position
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikStrength);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, new Vector3(rightFoot.position.x, rightFootSurfaceHight + footBaseOffset, rightFoot.position.z));

            //right foot rotation
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikStrength);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
        }

        if(leftFootIKActive)
        {
            //left foot position
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikStrength);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, new Vector3(leftFoot.position.x, leftFootSurfaceHight + footBaseOffset, leftFoot.position.z));

            //left foot rotation
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikStrength);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }
    public void CheckSurface()
    {
        RaycastHit hit;

        rightFootIKActive = false;
        
        if (Physics.Raycast(rightFoot.position, -Vector3.up, out hit, rightFoot.position.y - baseTransform.position.y - 0.001f, mask))
        {
            rightFootSurfaceHight = hit.point.y;
            rightFootRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rightFootIKActive = true;
        }
        Debug.DrawRay(rightFoot.position, -Vector3.up, Color.red);

        leftFootIKActive = false;
        
        if (Physics.Raycast(leftFoot.position, -Vector3.up, out hit, leftFoot.position.y - baseTransform.position.y - 0.001f, mask))
        {
            leftFootSurfaceHight = hit.point.y;
            leftFootRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            leftFootIKActive = true;
        }
        Debug.DrawRay(leftFoot.position, -Vector3.up, Color.red);
    }
    /*
    public void CheckSurface()
    {
        RaycastHit hit;

        int mask = 1 << 9;
        mask = ~mask;

        rightIKActive = false;
        Vector3 fromKneeToFootRight = rightFoot.position - rightKnee.position - new Vector3(0, footBaseOffset, 0);
        if (Physics.Raycast(rightKnee.position, fromKneeToFootRight, out hit, fromKneeToFootRight.magnitude, mask))
        {
            rightFootSurfaceHight = hit.point.y;
            rightFootRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rightIKActive = true;
            Debug.Log(hit.transform.gameObject);
        }
        Debug.DrawRay(rightKnee.position, fromKneeToFootRight, Color.red);

        leftIKActive = false;
        Vector3 fromKneeToFootLeft = leftFoot.position - leftKnee.position - new Vector3(0, footBaseOffset, 0);
        if (Physics.Raycast(leftKnee.position, fromKneeToFootLeft, out hit, fromKneeToFootLeft.magnitude, mask))
        {
            leftFootSurfaceHight = hit.point.y;
            leftFootRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            leftIKActive = true;
            Debug.Log(hit.transform.gameObject);
        }
        Debug.DrawRay(leftKnee.position, fromKneeToFootLeft, Color.red);

    }*/
    /*
    public void CheckSurface()
    {
        RaycastHit hit;

        int mask = 1 << 9;
        mask = ~mask;
        
        if (Physics.Raycast(rightFoot.position + new Vector3(0, rayCastOriginOffsetHeight, 0), -Vector3.up, out hit, 5, mask))
        {
            rightFootSurfaceHight = hit.point.y;
            rightFootRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            Debug.Log(rightFoot.position);
        }
        
        
        if (Physics.Raycast(leftFoot.position + new Vector3(0, rayCastOriginOffsetHeight, 0), -Vector3.up, out hit, 5, mask))
        {
            leftFootSurfaceHight = hit.point.y;
            leftFootRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            Debug.Log(leftFoot.position);
        }
        
    }*/
}
