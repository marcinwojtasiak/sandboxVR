using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [SerializeField] private GunBehaviour gunBehaviour;

    [SerializeField] private float backTranslation;
    [SerializeField] private float upAngle;
    [SerializeField] private float sideAngle; // takes random angle up to this angle

    [SerializeField] private float maxBackTranslation;
    [SerializeField] private float maxUpAngle;
    [SerializeField] private float maxSideAngle;

    [SerializeField] private float backReturn;
    [SerializeField] private float upReturn;
    [SerializeField] private float sideReturn;

    private Vector3 positionOffset = Vector3.zero;
    private Quaternion rotationOffset = Quaternion.identity;

    void Reset()
    {
        gunBehaviour = GetComponent<GunBehaviour>();
    }

    private void Update()
    {
        ReturnRecoil();

        if (gunBehaviour.interactable.mainHand)
        {
            gunBehaviour.interactable.mainHand.UpdateRecoil(transform.rotation * positionOffset, rotationOffset);
        }
    }

    public void ApplyRecoil()
    {
        positionOffset += Vector3.back * backTranslation;

        rotationOffset = rotationOffset * Quaternion.Euler(-upAngle, Random.Range(-sideAngle, sideAngle), 0);
    }

    public void ReturnRecoil()
    {
        float recoilUpAngle = rotationOffset.eulerAngles.x;
        recoilUpAngle = WrapAngle(recoilUpAngle);
        recoilUpAngle = Mathf.Clamp(recoilUpAngle + upReturn * Time.deltaTime, -maxUpAngle, 0);

        int sideAngleSign = 1;
        if (rotationOffset.eulerAngles.y < 0)
            sideAngleSign = -1;
        float recoilSideAngle = rotationOffset.eulerAngles.y;
        recoilSideAngle = WrapAngle(recoilSideAngle);
        recoilSideAngle = sideAngleSign * Mathf.Clamp(Mathf.Abs(recoilSideAngle) - sideReturn * Time.deltaTime, 0, maxSideAngle);

        float recoilForwardPosition = Mathf.Clamp(positionOffset.z + backReturn * Time.deltaTime, -maxBackTranslation, 0);

        positionOffset = new Vector3(0, 0, recoilForwardPosition);

        rotationOffset.eulerAngles = new Vector3(recoilUpAngle, recoilSideAngle, 0);
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
    /*

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform barrel;
    [SerializeField] private float backForce;
    [SerializeField] private float upForce;
    [SerializeField] private float maxSideForce;

    [SerializeField] private float stabilityModifier = 1f;
    [SerializeField] private float stabilityChangePerShot;
    [SerializeField] private float maxInsatbility;
    [SerializeField] private float recoverySpeed;

    [SerializeField] private float angularStabilityModifier = 1f;
    [SerializeField] private float angularStabilityChangePerShot;
    [SerializeField] private float maxAngularInsatbility;
    [SerializeField] private float angularRecoverySpeed;
    
    void Reset()
    {
        gunBehaviour = GetComponent<GunBehaviour>();
    }

    void Update()
    {
        //stabilityModifier = Mathf.Lerp(stabilityModifier, 1, recoverySpeed * Time.deltaTime);
        stabilityModifier += recoverySpeed * Time.deltaTime;
        if (stabilityModifier > 1f)
            stabilityModifier = 1f;

        //angularStabilityModifier = Mathf.Lerp(angularStabilityModifier, 1, angularRecoverySpeed * Time.deltaTime);
        angularStabilityModifier += angularRecoverySpeed * Time.deltaTime;
        if (angularStabilityModifier > 1f)
            angularStabilityModifier = 1f;

        if (gunBehaviour.interactable.mainHand)
        {
            gunBehaviour.interactable.mainHand.stabilityModifier = stabilityModifier;
            gunBehaviour.interactable.mainHand.angularStabilityModifier = angularStabilityModifier;
        }
    }

    public void ApplyRecoil()
    {
        ChangeStability();

        ApplyForces();
    }


    private void ApplyForces()
    {
        rigidbody.AddForceAtPosition(-barrel.forward * backForce, barrel.position, ForceMode.Impulse);

        rigidbody.AddForceAtPosition(barrel.up * upForce, barrel.position, ForceMode.Impulse);

        float sideForce = Random.Range(-maxSideForce, maxSideForce);
        rigidbody.AddForceAtPosition(barrel.right * sideForce, barrel.position, ForceMode.Impulse);
    }

    private void ChangeStability()
    {
        stabilityModifier -= stabilityChangePerShot;
        if (stabilityModifier < maxInsatbility)
            stabilityModifier = maxInsatbility;

        angularStabilityModifier -= angularStabilityChangePerShot;
        if (angularStabilityModifier < maxAngularInsatbility)
            angularStabilityModifier = maxAngularInsatbility;      
    }*/
}
