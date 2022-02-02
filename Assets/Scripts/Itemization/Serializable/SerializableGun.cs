using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableGun : SerializableItem // TODO: add serialization of attachments on the gun
{
    public SerializableMagazine magazine = null;

    public override GameObject Deserialize()
    {
        GameObject newObject = base.Deserialize();
        if (!newObject)
        {
            Debug.LogWarning("Trying to create a magazine from resource that doesn't have a script of type ItemData!");
            return null;
        }

        GunBehaviour gunBehaviour = newObject.GetComponent<GunBehaviour>();
        if (!gunBehaviour)
        {
            Debug.LogWarning("Trying to create a gun from resource that doesn't have GunBehaviour script!");
            UnityEngine.Object.Destroy(newObject);
            return null;
        }

        GameObject magObj;
        if(magazine != null)
        {
            magObj = magazine.Deserialize(); // if magObj has no Magazine script, Deserialize will return null
            if(magObj)
            {
                gunBehaviour.magazineSlot.AttachMag(magObj.GetComponent<Magazine>());
            }
        }

        return newObject;
    }
}
