using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources thisHand;
    [SerializeField] private SteamVR_Action_Boolean grabAction;
    [SerializeField] private SteamVR_Action_Boolean useAction;
    [SerializeField] private SteamVR_Action_Boolean touchpadClickAction;
    [SerializeField] private SteamVR_Behaviour_Skeleton handsSkeleton;

    [SerializeField] private InteractableObject heldObject;
    private ColliderRelay heldRelay;
    private IUsableObject heldUsable;
    [SerializeField] private List<ColliderRelay> hoveredObjects = new List<ColliderRelay>();

    [SerializeField] private Transform interactionPoint;


    public float stabilityModifier = 1;//wyjebac
    public float angularStabilityModifier = 1;//wyjebac
    private Vector3 recoilPosOffset = Vector3.zero;
    private Quaternion recoilRotOffset = Quaternion.identity;


    public bool ignoreHandSkeletonPosition; // use to set interaction offset
    void Start()
    {
        stabilityModifier = 1;
        angularStabilityModifier = 1;
    }

    void Update()
    {
        if (grabAction.GetStateDown(thisHand))
        {
            ColliderRelay closestInteractable = GetClosestInteractable();

            if (heldObject != null) //is holding something
            {
                InteractableObject droppedObj = DropObject();

                if(closestInteractable != null)
                {
                    SlotInteractions slot = closestInteractable.GetComponent<SlotInteractions>();
                    if (slot != null) //if drops an item while hovering over an empty inv slot
                    {
                        GameObject lefetoverObj = slot.AddItem(droppedObj.gameObject);
                        GrabObject(ColliderRelay.FindMainRelay(lefetoverObj));
                    }
                }
            }
            else //isn't holding anything
            {
                Debug.LogWarning(closestInteractable);
                if (closestInteractable != null)
                {
                    SlotInteractions slot = closestInteractable.GetComponent<SlotInteractions>();
                    CraftableItemDisplay craftableDisplay = closestInteractable.GetComponent<CraftableItemDisplay>();
                    Debug.LogWarning(slot);
                    Debug.LogWarning(craftableDisplay);
                    if (slot != null)
                    {
                        GrabObject(ColliderRelay.FindMainRelay(slot.RemoveItem()));
                    }
                    else if(craftableDisplay != null)
                    {
                        Debug.LogWarning("clicked crafting display");
                        GrabObject(ColliderRelay.FindMainRelay(craftableDisplay.Craft()));
                    }
                    else
                    {
                        GrabObject(closestInteractable);
                    }
                }
                
            }
        }

        if (useAction.GetStateDown(thisHand))
        {
            ColliderRelay closestInteractable = GetClosestInteractable();
            if (closestInteractable != null && heldObject == null)
            {
                if (closestInteractable.GetComponent<SlotInteractions>() != null) //if closest is an inventory slot - grab it
                {
                    GrabObject(closestInteractable);
                }
                else //if not - pick up to inv
                {
                    Inventory.localInventory.PickItem(closestInteractable.rootObject.gameObject);
                }
            }
            
        }

        if (useAction.GetStateUp(thisHand))
        {
            if (heldObject != null)
            {
                SlotInteractions heldSlot = heldObject.GetComponent<SlotInteractions>();
                if (heldSlot != null) //if letting go of the trigger and holding a slot in inventory - drop it
                {
                    heldSlot.SwapItems(heldSlot.FindSlotToSwap());
                    DropObject();
                }
            }
        }

        if(heldUsable != null)
        {
            GetInput();
        }
    }

    void FixedUpdate()
    {
        UpdateHeldObjectMovement();
    }

    protected const float MaxVelocityChange = 10f;
    protected const float VelocityMagic = 6000f;
    protected const float AngularVelocityMagic = 50f;
    protected const float MaxAngularVelocityChange = 20f;

    private void UpdateHeldObjectMovement()
    {
        if (heldObject)
        {
            if(heldObject.mainHand && heldObject.offHand == this)
            {
                return;
            }

            Vector3 velocityTarget, angularTarget;
            bool success = GetUpdatedAttachedVelocities(out velocityTarget, out angularTarget);
            if (success)
            {
                float maxAngularVelocityChange = MaxAngularVelocityChange / heldObject.objectsRigidbody.mass * angularStabilityModifier;
                float maxVelocityChange = MaxVelocityChange / heldObject.objectsRigidbody.mass * stabilityModifier;

                heldObject.objectsRigidbody.velocity = Vector3.MoveTowards(heldObject.objectsRigidbody.velocity, velocityTarget, maxVelocityChange);
                
                heldObject.objectsRigidbody.angularVelocity = Vector3.MoveTowards(heldObject.objectsRigidbody.angularVelocity, angularTarget, maxAngularVelocityChange);
            }
        }
    }

    private bool GetUpdatedAttachedVelocities(out Vector3 velocityTarget, out Vector3 angularTarget)
    {
        bool realNumbers = false;


        float velocityMagic = VelocityMagic;
        float angularVelocityMagic = AngularVelocityMagic;

        Vector3 targetItemPosition = TargetItemPosition();
        Vector3 positionDelta = (targetItemPosition - heldObject.objectsRigidbody.position);

        velocityTarget = positionDelta * velocityMagic * Time.deltaTime;

        if (float.IsNaN(velocityTarget.x) == false && float.IsInfinity(velocityTarget.x) == false)
        {
            //if (noSteamVRFallbackCamera)
            //    velocityTarget /= 10; //hacky fix for fallback
            realNumbers = true;
        }
        else
            velocityTarget = Vector3.zero;



        Quaternion targetItemRotation;
        if (heldObject.mainHand && heldObject.offHand) // is held with two hands
        {
            targetItemRotation = TargetItemRotationTwoHanded();
        }
        else // is held with one hand
        {
            targetItemRotation = TargetItemRotation();
        }

        Quaternion rotationDelta = targetItemRotation * Quaternion.Inverse(heldObject.transform.rotation);

        float angle;
        Vector3 axis;
        rotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        if (angle != 0 && float.IsNaN(axis.x) == false && float.IsInfinity(axis.x) == false)
        {
            angularTarget = angle * axis * angularVelocityMagic * Time.deltaTime;
            //angularTarget = recoilRotation * angularTarget; // add recoil rotation (nie dziala)

            //if (noSteamVRFallbackCamera)
            //    angularTarget /= 10; //hacky fix for fallback

            realNumbers &= true;
        }
        else
            angularTarget = Vector3.zero;

        return realNumbers;
    }

    private Vector3 TargetItemPosition()
    {
        Vector3 positionOffset = heldRelay.objectsPoser.GetBlendedPose(handsSkeleton.skeletonAction, thisHand).position;
        /*
        Vector3 positionOffset = heldRelay.interactionPositionOffset;

        if (thisHand == SteamVR_Input_Sources.LeftHand)
        {
            positionOffset = new Vector3(-positionOffset.x, positionOffset.y, positionOffset.z);
        }*/

        return transform.position + (transform.rotation * positionOffset) + recoilPosOffset;
        //return transform.position + (TargetItemRotation() * positionOffset) + recoilPosOffset;
    }

    private Quaternion TargetItemRotation()
    {
        Quaternion rotationOffset = heldRelay.objectsPoser.GetBlendedPose(handsSkeleton.skeletonAction, thisHand).rotation;
        /*
        Quaternion rotationOffset = Quaternion.Euler(heldRelay.interactionRotationOffset);

        if (thisHand == SteamVR_Input_Sources.LeftHand)
        {
            rotationOffset = new Quaternion(-rotationOffset.x, rotationOffset.y, rotationOffset.z, -rotationOffset.w);
        }*/

        return transform.rotation * rotationOffset * recoilRotOffset;
    }

    private Quaternion TargetItemRotationTwoHanded()
    {
        /*Quaternion rotationOffset = Quaternion.Euler(heldObject.mainHand.heldRelay.interactionRotationOffset); //?
        if (heldObject.offHand.thisHand == SteamVR_Input_Sources.LeftHand)
        {
            rotationOffset = new Quaternion(-rotationOffset.x, rotationOffset.y, rotationOffset.z, -rotationOffset.w);
        }*/

        /*Vector3 offHandPositionOffset = heldObject.offHand.heldRelay.interactionPositionOffset;
        if (heldObject.offHand.thisHand == SteamVR_Input_Sources.LeftHand)
        {
            offHandPositionOffset = new Vector3(-offHandPositionOffset.x, offHandPositionOffset.y, offHandPositionOffset.z);
        }*/

        Vector3 vectorBetweenHands = heldObject.offHand.transform.position - heldObject.mainHand.transform.position;
        if (heldObject.twoHandInvertForward)
            vectorBetweenHands *= -1;
        Quaternion faceAtControllerRotation = Quaternion.LookRotation(vectorBetweenHands, interactionPoint.up); // heldObject.mainHand.transform.forward

        Quaternion targetRotation = faceAtControllerRotation;

        return targetRotation * recoilRotOffset;
    }


    void OnTriggerEnter(Collider coll)
    {
        ColliderRelay touched = coll.GetComponentInParent<ColliderRelay>();
        if(touched != null)
            hoveredObjects.Add(touched);
    }

    void OnTriggerExit(Collider coll)
    {
        hoveredObjects.Remove(coll.GetComponentInParent<ColliderRelay>());
    }

    private ColliderRelay GetClosestInteractable()
    {
        if (hoveredObjects.Count == 0)
        {
            return null;
        }
        while (hoveredObjects[0] == null) // make sure first object exists
        {
            hoveredObjects.RemoveAt(0);
            if (hoveredObjects.Count == 0)
            {
                return null;
            }
        }

        ColliderRelay closest = null;
        float closestDistance = float.MaxValue;
        

        for (int i = 0; i < hoveredObjects.Count; i++)
        {
            if(hoveredObjects[i] == null || !hoveredObjects[i].GetComponent<Collider>().enabled) // remove non-existent
            {
                hoveredObjects.RemoveAt(i);
                i--;
                continue;
            }
            if(hoveredObjects[i] == heldRelay) //ignore held object and non-interactable objects             || !hoveredObjects[i].CanBeGrabbed()
            {
                continue;
            }

            float newDistance = Vector3.Distance(hoveredObjects[i].transform.position, interactionPoint.position);
            if(newDistance < closestDistance)
            {
                closestDistance = newDistance;
                closest = hoveredObjects[i];
            }
        }
        return closest;
    }

    public void UpdateRecoil(Vector3 positionOffset, Quaternion rotationOffset)
    {
        recoilPosOffset = positionOffset;
        recoilRotOffset = rotationOffset;
    }

    /// <summary>
    /// Function that forces hand to pick up an object (if it is possible to be picked up). Drops any held objects and sets skeleton to held object's poser.
    /// </summary>
    /// <param name="grabbedCollider">Object to be picked up.</param>
    public void GrabObject(ColliderRelay grabbedCollider)
    {
        if (!grabbedCollider || !grabbedCollider.CanBeGrabbed())
            return;

        if(heldObject != null)
        {
            DropObject();
        }

        Mirror.NetworkIdentity networkIdentity = grabbedCollider.rootObject.GetComponent<Mirror.NetworkIdentity>();
        if (networkIdentity && !networkIdentity.hasAuthority && grabbedCollider.rootObject.IsHeldByAnyPlayer()) 
        {
            return;
        }
        if (networkIdentity)
        {
            PlayerStats.current.CmdGrabObject(networkIdentity);
            NetworkTransform networkTransform = networkIdentity.GetComponent<NetworkTransform>();
            if(networkTransform)
                networkTransform.GetTemporaryAuth();

        }

        heldRelay = grabbedCollider;
        heldUsable = grabbedCollider.rootObject.GetComponent<IUsableObject>();
        heldObject = grabbedCollider.rootObject;
        heldObject.objectsRigidbody.maxAngularVelocity = 30;


        if (grabbedCollider.isMainGrip) // Main hand grabbing
        {
            heldObject.mainHand = this;
        }
        else // Off hand grabbing
        {
            heldObject.offHand = this;
        }

        if (!ignoreHandSkeletonPosition)
        {
            Vector3 previousPosition = heldObject.transform.position;
            Quaternion previousRotation = heldObject.transform.rotation;

            heldObject.transform.position = TargetItemPosition();
            heldObject.transform.rotation = TargetItemRotation();
            //heldObject.transform.position = transform.position + (transform.rotation * grabbedCollider.interactionPositionOffset);
            //heldObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbedCollider.interactionRotationOffset);

            handsSkeleton.transform.parent = grabbedCollider.transform;

            heldObject.transform.position = previousPosition;
            heldObject.transform.rotation = previousRotation;
        }

        ChangePoseToMatchObject(grabbedCollider.objectsPoser);
    }

    /// <summary>
    /// Returns object that has been dropped, can be null. Resets hand's skeleton.
    /// </summary>
    public InteractableObject DropObject()
    {
        if(heldObject.mainHand == this) //this Hand is the main hand
        {
            heldObject.mainHand = null;
        }
        else if(heldObject.offHand == this) //this Hand is the off hand
        {
            heldObject.offHand = null;
        }

        InteractableObject droppedObject = heldObject;

        if (heldObject.mainHand == null && heldObject.offHand == null)
        {
            heldObject.objectsRigidbody.maxAngularVelocity = 7;

            Mirror.NetworkIdentity networkIdentity = droppedObject.GetComponent<Mirror.NetworkIdentity>();
            if (networkIdentity)
            {
                PlayerStats.current.CmdUnassignObjectAuthority(networkIdentity);
            }
        }

        heldObject = null;
        heldRelay = null;
        heldUsable = null;


        handsSkeleton.transform.parent = transform;
        handsSkeleton.transform.localPosition = Vector3.zero;
        handsSkeleton.transform.localRotation = Quaternion.identity;

        ChangePoseToMatchDefaultHand();

        return droppedObject;
    }

    private void ChangePoseToMatchObject(SteamVR_Skeleton_Poser objectsPoser)
    {
        handsSkeleton.BlendToPoser(objectsPoser);
    }

    private void ChangePoseToMatchDefaultHand()
    {
        handsSkeleton.BlendToSkeleton();
    }


    //-----------------------------------------------------------------------------------------------

    private void GetInput()
    {
        if (heldObject.mainHand != this)
            return;
        if(useAction.GetStateDown(thisHand))
        {
            heldUsable.UseClick();
        }
        if(useAction.GetState(thisHand))
        {
            heldUsable.UseHold();
        }
        if(touchpadClickAction.GetStateDown(thisHand))
        {
            heldUsable.TouchpadClick();
        }
    }

}
