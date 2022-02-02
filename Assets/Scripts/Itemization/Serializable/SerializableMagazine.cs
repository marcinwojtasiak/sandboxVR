using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableMagazine : SerializableItem
{
    public int ammo;

    public override GameObject Deserialize()
    {
        GameObject newObject = base.Deserialize();
        if (!newObject)
        {
            Debug.LogWarning("Trying to create a magazine from resource that doesn't have a script of type ItemData!");
            return null;
        }
        Magazine mag = newObject.GetComponent<Magazine>();
        if (!mag)
        {
            Debug.LogWarning("Trying to create a magazine from resource that doesn't have Magazine script!");
            UnityEngine.Object.Destroy(newObject);
            return null;
        }
        mag.ammo = ammo;
        
        return newObject;
    }
}
