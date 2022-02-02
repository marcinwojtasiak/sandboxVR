using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    [SerializeField] private bool active = false;

    [SerializeField] private Transform animRoot; // <-------these should have identical hierarchies
    [SerializeField] private Transform physicsRoot; // <---|

    [SerializeField] private List<Transform> animBones = new List<Transform>();
    [SerializeField] private List<Rigidbody> physicBones = new List<Rigidbody>();

    void Start()
    {
        List<Transform> animTrans = new List<Transform>();
        List<Transform> physicTrans = new List<Transform>();

        foreach (Transform bone in physicsRoot.GetComponentsInChildren<Transform>())
        {
            physicTrans.Add(bone);
        }
        foreach (Transform bone in animRoot.GetComponentsInChildren<Transform>())
        {
            animTrans.Add(bone);
        }

        for(int i=0; i<physicTrans.Count; i++)
        {
            Rigidbody rb = physicTrans[i].GetComponent<Rigidbody>();
            if (rb)
            {
                physicBones.Add(rb);
                animBones.Add(animTrans[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            UpdateBonesMovement();
    }

    protected const float MaxVelocityChange = 10f;
    protected const float VelocityMagic = 6000f;
    protected const float AngularVelocityMagic = 50f;
    protected const float MaxAngularVelocityChange = 20f;

    private void UpdateBonesMovement()
    {
        for (int i = 0; i < physicBones.Count; i++)
        {
            Vector3 velocityTarget, angularTarget;
            bool success = GetUpdatedAttachedVelocities(i, out velocityTarget, out angularTarget);
            if (success)
            {
                float maxAngularVelocityChange = MaxAngularVelocityChange;
                float maxVelocityChange = MaxVelocityChange;

                physicBones[i].velocity = Vector3.MoveTowards(physicBones[i].velocity, velocityTarget, maxVelocityChange);

                physicBones[i].angularVelocity = Vector3.MoveTowards(physicBones[i].angularVelocity, angularTarget, maxAngularVelocityChange);
            }
        }
    }

    private bool GetUpdatedAttachedVelocities(int boneIndex, out Vector3 velocityTarget, out Vector3 angularTarget)
    {
        bool realNumbers = false;


        float velocityMagic = VelocityMagic;
        float angularVelocityMagic = AngularVelocityMagic;

        Vector3 targetItemPosition = animBones[boneIndex].position;
        Vector3 positionDelta = targetItemPosition - physicBones[boneIndex].position;

        velocityTarget = positionDelta * velocityMagic * Time.deltaTime;

        if (float.IsNaN(velocityTarget.x) == false && float.IsInfinity(velocityTarget.x) == false)
        {
            //if (noSteamVRFallbackCamera)
            //    velocityTarget /= 10; //hacky fix for fallback
            realNumbers = true;
        }
        else
            velocityTarget = Vector3.zero;



        Quaternion targetItemRotation = animBones[boneIndex].rotation;
        Quaternion rotationDelta = targetItemRotation * Quaternion.Inverse(physicBones[boneIndex].rotation);

        float angle;
        Vector3 axis;
        rotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        if (angle != 0 && float.IsNaN(axis.x) == false && float.IsInfinity(axis.x) == false)
        {
            angularTarget = angle * axis * angularVelocityMagic * Time.deltaTime;
            //angularTarget = recoilRotation * angularTarget; // add recoil rotation (nie dziala)

            //if (noSteamVRFallbackCamera)
            //    angularTarget /= 10; //hacky fix for fallback

            realNumbers &= true;
        }
        else
            angularTarget = Vector3.zero;

        return realNumbers;
    }
}
