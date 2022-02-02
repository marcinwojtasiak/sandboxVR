using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShotInfo
{
    private GunBehaviour owner;
    public Vector3 shotOrigin;
    public Quaternion originalRotation;
    public Vector3 shotDirection;
    public float force;

    public ShotInfo(GunBehaviour owner, Vector3 origin, Quaternion rotation, Vector3 direction, float force)
    {
        this.owner = owner;
        this.shotOrigin = origin;
        this.originalRotation = rotation;
        this.shotDirection = direction;
        this.force = force;
    }

    public GunBehaviour GetOwner()
    {
        return owner;
    }
    public Vector3 GetShotOrigin()
    {
        return shotOrigin;
    }
    public Quaternion GetRotation()
    {
        return originalRotation;
    }
    public Vector3 GetShotDirection()
    {
        return shotDirection;
    }
    public float GetForce()
    {
        return force;
    }

}