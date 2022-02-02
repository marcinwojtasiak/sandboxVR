using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources movementSource;
    [SerializeField] private SteamVR_Action_Vector2 moveInput;

    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform controller;

    private float speed = 0.02f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        cameraRig.Translate(GetDirection() * moveInput.GetAxis(movementSource).magnitude * speed);
    }

    private Vector3 GetDirection()
    {
        Vector3 controllerDirOnGround = Vector3.ProjectOnPlane(controller.forward, Vector3.up);
        float angleOnTouchpad = Vector2.SignedAngle(Vector2.up, moveInput.GetAxis(movementSource));
        Quaternion rotationFromTPad = Quaternion.AngleAxis(angleOnTouchpad, Vector3.down);
        return rotationFromTPad * controllerDirOnGround;
    }
}
