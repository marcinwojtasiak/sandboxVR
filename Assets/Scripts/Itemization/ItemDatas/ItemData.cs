using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour //data of a particular item instance
{
    public string nameID;
    public int maxStackSize { get; private set; }
    public int stackSize = 1;

    void Awake()
    {
        if(nameID == "")
        {
            nameID = name;
        }
        maxStackSize = ((Item)Resources.Load("Items/" + nameID)).maxStackSize;
    }

    public virtual SerializableItem Serialize()
    {
        return new SerializableItem
        {
            nameID = nameID,
            stackSize = stackSize
        };
    }
}
