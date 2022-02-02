using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    public static Inventory localInventory;

    [SerializeField] private int numberOfSlots = 20;

    public GameObject invUI;
    [SerializeField] private SlotInteractions[] slots;

    [SerializeField] private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        //TODO: NETWORKING     if(isLocal)
        localInventory = this;
        ConnectToUI();
    }

    void Update()
    {
#if UNITY_EDITOR //for testing on keyboard
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            int mask = 1 << 14;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20f, mask);
            if (hit.transform != null)
            {
                PickItem(hit.transform.gameObject);
                Debug.Log(hit.transform.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isOpen)
                CloseInventory();
            else
                OpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            slots[0].SwapItems(slots[1]);
        }
#endif //-----------------------------
    }

    public bool PickItem(GameObject item)
    {    
        ItemData data = item.GetComponent<ItemData>();
        if (!data)
            return false;
        if(data.maxStackSize <= 0)
        {
            Debug.LogWarning("Max stack can't be lower than 1! " + data);
            return false;
        }
        
        if (data.maxStackSize == 1) //if non stackable - put in first free slot
        {
            for (int i = 0; i < slots.Length; i++)
            {
                Debug.Log(slots[i].storedItem);
                if (slots[i].storedItem == null)
                {
                    slots[i].AddItem(item);
                    return true;
                }
            }
        }
        else //else - try to find such an item in eq and stack it there (up to maxStackSize)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].storedItem != null && slots[i].storedItem.nameID == data.nameID)
                {
                    if (data.maxStackSize - slots[i].storedItem.stackSize < data.stackSize)
                    {
                        data.stackSize -= (data.maxStackSize - slots[i].storedItem.stackSize);
                        slots[i].storedItem.stackSize = data.maxStackSize;
                    }
                    else if (data.maxStackSize - slots[i].storedItem.stackSize >= data.stackSize)
                    {
                        slots[i].storedItem.stackSize += data.stackSize;
                        data.stackSize = 0;
                    }

                    if (data.stackSize <= 0) //if no more left - end
                    {
                        Destroy(item);
                        CraftingMenu.localCraftingMenu.UpdateList();
                        return true;
                    }
                }
            }
            if (data.stackSize > 0) //is something left - put in first free slot 
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].storedItem == null)
                    {
                        slots[i].AddItem(item);
                        return true;
                    }
                }
            }
        }

        CraftingMenu.localCraftingMenu.UpdateList();
        return false;
    }

    public int GetItemQuantity(string name)
    {
        int quantity = 0;
        foreach(SlotInteractions slot in slots)
        {
            if(slot.storedItem != null && slot.storedItem.nameID == name)
            {
                quantity += slot.storedItem.stackSize;
            }
        }
        return quantity;
    }

    public GameObject RemoveItemAmount(string name, int amount)
    {
        foreach (SlotInteractions slot in slots)
        {
            if (slot.storedItem != null && slot.storedItem.nameID == name)
            {
                return slot.RemoveItem(amount);
            }
        }
        return null;
    }

    public void ChangeInventoryState()
    {
        if (isOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    private void OpenInventory()
    {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Camera.main.transform.rotation.eulerAngles.y, 0));
        isOpen = true;
        animator.SetBool("isOpen", true);
    }

    private void CloseInventory()
    {
        foreach(SlotInteractions slot in slots)
        {
            slot.StopHolding();
        }
        isOpen = false;
        animator.SetBool("isOpen", false);
    }

    private void ConnectToUI()
    {
        slots = new SlotInteractions[numberOfSlots];
        int i = 0;
        foreach (Transform slot in invUI.transform)
        {
            slots[i] = slot.GetComponent<SlotInteractions>();
            i++;
        }
    }

}
/*
[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Inventory inventory = (Inventory)target;
        if (GUILayout.Button("Get inv"))
        {
            inventory.ConnectToUI();
        }
    }
}*/