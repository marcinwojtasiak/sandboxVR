using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerRepresentation : NetworkBehaviour
{
    //public Transform head;
    //public Transform rightHand;
    //public Transform leftHand;

    public Transform localRig;
    public Transform localHeadset;
    public Transform localRightHand;
    public Transform localLeftHand;

    public GameObject[] localInvisibleObjects;

    private float maxHeadRotation = 90.0f;

    void Start()
    {
        PopulateLocalFields();
        Inventory.localInventory.transform.parent = transform;
        Inventory.localInventory.transform.localPosition = Vector3.zero;
    }

    public override void OnStartLocalPlayer()
    {
        foreach (GameObject item in localInvisibleObjects)
        {
            item.SetActive(false);
        }
    }

    void Update()
    {
        PopulateLocalFields(); // TODO potential optimalization

        //UpdateLocalPlayerPositions();

        UpdateBody();
    }
    /*
    private void UpdateLocalPlayerPositions()
    {
        if(isLocalPlayer)
        {
            head.position = localHeadset.position;
            head.rotation = localHeadset.rotation;
            rightHand.position = localRightHand.position;
            rightHand.rotation = localRightHand.rotation;
            leftHand.position = localLeftHand.position;
            leftHand.rotation = localLeftHand.rotation;
        }
    }*/

    private void UpdateBody()
    {
        if (!isLocalPlayer)
            return;

        //position
        transform.position = new Vector3(localHeadset.position.x, localRig.position.y, localHeadset.position.z);

        //rotation
        float angle = Vector3.SignedAngle(transform.forward, localHeadset.forward, transform.up);
        if (Mathf.Abs(angle) > maxHeadRotation)
        {
            transform.Rotate(new Vector3(0, (angle - maxHeadRotation) % (maxHeadRotation * 2), 0));
        }
    }

    private void PopulateLocalFields()
    {
        if(!localRig)
            localRig = GameObject.Find("[CameraRig]").transform;
        if (!localHeadset)
            localHeadset = GameObject.Find("Camera").transform;
        if(!localRightHand)
            localRightHand = GameObject.Find("vr_glove_right").transform;
        if(!localLeftHand)
            localLeftHand = GameObject.Find("vr_glove_left").transform;
    }

    private void CmdStartGrabbingItem(GameObject grabbedItem, HoldType howHeld)
    {
        Debug.Log("Assigning authority: " + grabbedItem.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient));
        Debug.Log(grabbedItem.GetComponent<InteractableObject>(), grabbedItem);
    }

    enum HoldType
    {
        NoMultiHold, MainRight, MainLeft
    }
}
