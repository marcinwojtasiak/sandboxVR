using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftableObject : ScriptableObject
{
    public Item asset;
    public List<Item> components = new List<Item>();
    public List<int> quantity = new List<int>();
}
