using UnityEngine;

public interface IHittable
{
    void Hit(float intensity, Collision collision);
}