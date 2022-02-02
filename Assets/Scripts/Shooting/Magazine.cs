using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour {

    public InteractableObject interactable;

    public int capacity = 30;
    public int ammo = 30;
    public AmmoType ammoType;
    public AmmoBonus ammoBonus;

	void Reset ()
    {
        interactable = GetComponent<InteractableObject>();
	}

    public void SetNonInteractable(bool value)
    {
        interactable.SetNonGrabbable(value);
        interactable.SetStatic(value);
        SetAllCollidersStatus(!value);
    }

    private void SetAllCollidersStatus(bool active)
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = active;
        }
    }

    public enum AmmoType
    {
        Pistol, Shotgun, Rifle, Sniper, Heavy, Grenade, Rocket, Laser, Arrow, Thrown
    }
    public enum AmmoBonus
    {
        Basic, Overloaded, Explosive, Hitscan
    }
}
