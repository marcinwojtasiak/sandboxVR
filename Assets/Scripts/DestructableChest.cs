using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableChest : MonoBehaviour, IDamagable, IHittable
{
    [SerializeField] private GameObject destroyedVersion;	// Reference to the shattered version of the object
    [SerializeField] private float hp = 1;


    public void Fracture()
    {
        GameObject go = Instantiate(destroyedVersion, transform.position, transform.rotation);

        foreach (Rigidbody rb in go.GetComponentsInChildren<Rigidbody>())
        {
            rb.velocity = GetComponent<Rigidbody>().velocity;
        }
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        hp -= damage;
        if (hp <= 0f)
            Fracture();
    }

    public void Hit(float intensity, Collision collision)
    {
        Damage(1f);
    }
}

