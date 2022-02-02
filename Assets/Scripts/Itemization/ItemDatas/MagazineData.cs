using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineData : ItemData
{
    public override SerializableItem Serialize()
    {
        Magazine mag = GetComponent<Magazine>();
        if (!mag)
        {
            Debug.LogWarning("Trying to serialize an object without Magazine script as magazine!");
            return null;
        }
        return new SerializableMagazine
        {
            nameID = nameID,
            stackSize = stackSize,
            ammo = mag.ammo
        };
    }
}
