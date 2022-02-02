using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class InteractableObject : MonoBehaviour
{
    public InteractionType interactionType;

    public bool twoHandInvertForward = false;

    //[HideInInspector]
    public Hand mainHand;
    //[HideInInspector]
    public Hand offHand;

    [SerializeField] private bool nonGrabbable = false;
    [SerializeField] private bool isStatic = false;

    private bool isHeldByPlayer = false;


    public Rigidbody objectsRigidbody;

    private void Start()
    {
        SetNonGrabbable(nonGrabbable); //workaround to set right values when changing bool to true in inspector
        SetStatic(isStatic);
    }

    private void Update()
    {
        //Debug.Log(netIdentity.clientAuthorityOwner);
    }
    void Reset()
    {
        objectsRigidbody = GetComponentInParent<Rigidbody>();
        AddColliderRelay();
    }

    private void AddColliderRelay()
    {
        ColliderRelay relay = gameObject.AddComponent<ColliderRelay>();
        relay.rootObject = this;
    }


    //--------------------------------------------------------------------
    public void SetStatic(bool value)
    {
        isStatic = value;
        objectsRigidbody.isKinematic = value;
    }
    public bool IsStatic()
    {
        return isStatic;
    }
    public bool IsHeldByAnyPlayer()
    {
        return isHeldByPlayer;
    }
    
    public void SetNonGrabbable(bool value)
    {
        nonGrabbable = value;
        if(value == true)
        {
            if(mainHand)
                mainHand.DropObject();
            if (offHand)
                offHand.DropObject();
        }
    }
    public bool NonGrabbable()
    {
        return nonGrabbable;
    }

    [ClientRpc]
    public void RpcSetObjectAsHeld(bool isHeld)
    {
        isHeldByPlayer = isHeld;
    }
}

public enum InteractionType
{
    StandardHold, NoGripHold
}