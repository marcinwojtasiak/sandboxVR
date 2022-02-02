using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbConstraints : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private bool freezePosition;
    [SerializeField] private Vector2 forward;
    [SerializeField] private Vector2 up;
    [SerializeField] private Vector2 right;

    [SerializeField] private bool freezeRotation;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Awake()
    {
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
    }

    void Update()
    {
        if(freezePosition)
        {
            BlockPosition();
        }
        if(freezeRotation)
        {
            BlockRotation();
        }

        
    }

    void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void BlockPosition()
    {
        if (transform.localPosition.x <= defaultPosition.x - right.x || transform.localPosition.x >= defaultPosition.x + right.y)
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, rigidbody.velocity.z);
        }

        if (transform.localPosition.y <= defaultPosition.y - up.x || transform.localPosition.y >= defaultPosition.y + up.y)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (transform.localPosition.z <= defaultPosition.z - forward.x || transform.localPosition.z >= defaultPosition.z + forward.y)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0);
        }

        transform.localPosition = new Vector3(
        Mathf.Clamp(transform.localPosition.x, defaultPosition.x - right.x, defaultPosition.x + right.y),
        Mathf.Clamp(transform.localPosition.y, defaultPosition.y - up.x, defaultPosition.y + up.y),
        Mathf.Clamp(transform.localPosition.z, defaultPosition.z - forward.x, defaultPosition.z + forward.y));
    }

    private void BlockRotation()
    {
        transform.localRotation = defaultRotation;
    }
}
