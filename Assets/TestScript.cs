using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TestScript : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(target.position - rb.position);
    }


    /*protected bool GetUpdatedAttachedVelocities(AttachedObject attachedObjectInfo, out Vector3 velocityTarget, out Vector3 angularTarget)
    {
        bool realNumbers = false;


        float velocityMagic = VelocityMagic;
        float angularVelocityMagic = AngularVelocityMagic;

        Vector3 targetItemPosition = TargetItemPosition(attachedObjectInfo);
        Vector3 positionDelta = (targetItemPosition - attachedObjectInfo.attachedRigidbody.position);
        velocityTarget = (positionDelta * velocityMagic * Time.deltaTime);

        if (float.IsNaN(velocityTarget.x) == false && float.IsInfinity(velocityTarget.x) == false)
        {
            if (noSteamVRFallbackCamera)
                velocityTarget /= 10; //hacky fix for fallback

            realNumbers = true;
        }
        else
            velocityTarget = Vector3.zero;


        Quaternion targetItemRotation = TargetItemRotation(attachedObjectInfo);
        Quaternion rotationDelta = targetItemRotation * Quaternion.Inverse(attachedObjectInfo.attachedObject.transform.rotation);


        float angle;
        Vector3 axis;
        rotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        if (angle != 0 && float.IsNaN(axis.x) == false && float.IsInfinity(axis.x) == false)
        {
            angularTarget = angle * axis * angularVelocityMagic * Time.deltaTime;

            if (noSteamVRFallbackCamera)
                angularTarget /= 10; //hacky fix for fallback

            realNumbers &= true;
        }
        else
            angularTarget = Vector3.zero;

        return realNumbers;
    }*/
}
