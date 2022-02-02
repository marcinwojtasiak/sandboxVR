using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_Skeleton_Poser))]
public class ColliderRelay : MonoBehaviour
{
    public InteractableObject rootObject;
    public SteamVR_Skeleton_Poser objectsPoser;
    public bool isMainGrip = true;

    public Vector3 interactionPositionOffset; // wywalic
    public Vector3 interactionRotationOffset; //

    void Reset() // Code executed on adding script
    {
        objectsPoser = GetComponentInParent<SteamVR_Skeleton_Poser>();
        InteractableObject interactable = GetComponentInParent<InteractableObject>();
        if (interactable)
        {
            rootObject = interactable;
        }
    }

    void Start()
    {
        if (rootObject == null)
        {
            Debug.LogWarning("No root object on the ColliderRelay script", this);
        }
    }

    public bool CanBeGrabbed()
    {
        if (rootObject.mainHand && isMainGrip)
            return false;

        if (rootObject.offHand && !isMainGrip)
            return false;

        if (rootObject.NonGrabbable())
            return false;
        return true;
    }

    public static ColliderRelay FindMainRelay(GameObject obj)
    {
        if (obj != null)
        {
            ColliderRelay[] colliderRelays = obj.GetComponentsInChildren<ColliderRelay>();
            foreach (ColliderRelay relay in colliderRelays)
            {
                if (relay.isMainGrip)
                {
                    return relay;
                }
            }
        }
        return null;
    }

    // Redundant code, may be useful later. 
    // Removes glitchy doubles of colliders that may have been added by multiple collisions
    /*public void RemoveAll(List<ColliderRelay> relays)
    {
        for (int i = 0; i < relays.Count; i++)
        {
            if (relays[i] == this)
            {
                relays.Remove(this);
                i--;
            }
        }
    }*/
}