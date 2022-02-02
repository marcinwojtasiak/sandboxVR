using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitDetection : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagableObject = collision.gameObject.transform.root.GetComponent<IDamagable>();
        if (damagableObject != null)
        {
            GetComponent<Bullet>().GetShotInfo().GetOwner().UpdateHit(collision);

            Destroy(gameObject);
        }
    }
}
