using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableItem //stores all data needed to restore an instance if a GameObject
{
    public string nameID;
    public int stackSize;

    public virtual GameObject Deserialize() // creates new GameObject from given SerializableData
    {
        Item resource = (Item)Resources.Load("Items/" + nameID);
        if(!resource)
        {
            Debug.LogWarning("Trying to load a resource with ID of "+ nameID +" which doesn't exist!");
            return null;
        }
        GameObject newObject = UnityEngine.Object.Instantiate(resource.prefab);
        ItemData itemData = newObject.GetComponent<ItemData>();
        if(!itemData)
        {
            Debug.LogWarning("Trying to deserialize object without ItemData!");
            return null;
        }
        newObject.GetComponent<ItemData>().stackSize = stackSize;

        return newObject;
    }
}
