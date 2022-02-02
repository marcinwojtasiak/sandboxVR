using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject // item asset, stores references needed to instanitate an object, objects name is its identificator
{
    public int maxStackSize;
    public GameObject prefab;
    public GameObject model;
    
}
