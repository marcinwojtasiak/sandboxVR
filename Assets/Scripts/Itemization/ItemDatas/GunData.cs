using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : ItemData
{
    public override SerializableItem Serialize()
    {
        SerializableGun serialized = new SerializableGun();

        MagazineData magData = GetComponentInChildren<MagazineData>(); 
        if (magData)
        {
            serialized.magazine = (SerializableMagazine)magData.Serialize();
        }

        serialized.nameID = nameID;
        serialized.stackSize = stackSize;
        return serialized;
    }
}
