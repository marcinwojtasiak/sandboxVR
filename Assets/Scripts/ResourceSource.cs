using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour, IHittable
{
    [SerializeField] private ItemData resourcePrefab; // assign in inspector
    [SerializeField] private int maxResourcePerHit = 20;
    [SerializeField] private int initialResource = 100;

    private int resource;

    private void Start()
    {
        resource = initialResource;
    }

    public void Hit(float intensity, Collision collision)
    {
        int amount = (int)(intensity * maxResourcePerHit);
        resource -= amount;
        SpawnResource(amount, collision.GetContact(0).point);
    }

    private void SpawnResource(int amount, Vector3 position)
    {
        GameObject res = Instantiate(resourcePrefab.gameObject, position, Quaternion.identity);
        res.GetComponent<ItemData>().stackSize = amount;
        Inventory.localInventory.PickItem(res);
    }
}
