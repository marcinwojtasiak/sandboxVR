using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotInteractions : MonoBehaviour {

    [SerializeField] private InteractableObject interactable;

    [SerializeField] private Vector3 slotInventoryOffset;
    //public bool isHeld;

    private TextMeshPro stackNumber;
    private GameObject miniature;

    public SerializableItem storedItem;
    //public string itemID = null;
    //public GameObject item;
    //public Vector3 scale;
    //public ItemData data;

    private const float snapDistance = 1.0f;
    private const float minSwapDistance = 1.0f;

    private List<SlotInteractions> touchedSlots = new List<SlotInteractions>();

    void Awake()
    {
        slotInventoryOffset = transform.localPosition;
    }

    void Start()
    {
        interactable = GetComponent<InteractableObject>();
        stackNumber = gameObject.GetComponentInChildren<TextMeshPro>();
        storedItem = null;
    }

    void Update()
    {
        if(!interactable.mainHand && !interactable.offHand) // is not held
        {
            interactable.objectsRigidbody.isKinematic = true;

            transform.localPosition = Vector3.Lerp(transform.localPosition, slotInventoryOffset, Time.deltaTime * 10f);
            if((slotInventoryOffset - transform.localPosition).magnitude < snapDistance)
            {
                transform.localPosition = slotInventoryOffset;
            }

            transform.localRotation = Quaternion.identity;// Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * 100);
        }
        else // is held
        {
            interactable.objectsRigidbody.isKinematic = false;
        }

        if (storedItem != null && storedItem.stackSize > 1)
            stackNumber.text = storedItem.stackSize.ToString();
        else
            stackNumber.text = "";
    }

    public GameObject AddItem(GameObject obj) // return obj that couldn't be added or the one that was replaced
    {
        ItemData data = obj.GetComponent<ItemData>();
        if (data == null)
        {
            Debug.LogWarning(obj + " is missing an ItemData script");
            return obj;
        }

        Item item = (Item)Resources.Load("Items/" + data.nameID);
        if (item == null)
        {
            Debug.LogWarning("Missing an asset for object: " + data);
            return obj;
        }

        GameObject model = item.model;
        if (!model)
        {
            Debug.LogWarning("No model for item asset: " + item.name);
            return obj;
        }

        if (storedItem != null && data.nameID == storedItem.nameID) // if it's the same item then just change stacks sizes
        {
            if(item.maxStackSize - storedItem.stackSize < data.stackSize)
            {
                data.stackSize -= item.maxStackSize - storedItem.stackSize;
                storedItem.stackSize = item.maxStackSize;
                CraftingMenu.localCraftingMenu.UpdateList();
                return obj;
            }
            else
            {
                storedItem.stackSize += data.stackSize;
                data.stackSize = 0;
                Destroy(obj);
                CraftingMenu.localCraftingMenu.UpdateList();
                return null;
            }
        }

        GameObject previousItem = null;
        if (storedItem != null)
            previousItem = RemoveItemStack();
        storedItem = data.Serialize();// new ItemSerializer().SerializeData(data);
        Destroy(obj);

        Quaternion rotation = model.transform.localRotation;
        miniature = Instantiate(model, transform.position, model.transform.rotation, transform);
        miniature.transform.localRotation = rotation;

        CraftingMenu.localCraftingMenu.UpdateList();
        return previousItem;
    }

    public GameObject RemoveItemStack()
    {
        if (storedItem == null)
            return null;

        GameObject removedItemStack = storedItem.Deserialize(); // new ItemSerializer().DeserializeData(storedItem);
        removedItemStack.transform.position = gameObject.transform.position;
        removedItemStack.transform.rotation = gameObject.transform.rotation;

        Clear();

        CraftingMenu.localCraftingMenu.UpdateList();
        return removedItemStack;
    }
    public GameObject RemoveItem()
    {
        if (storedItem == null)
            return null;

        if(storedItem.stackSize == 1)
        {
            return RemoveItemStack();
        }
        else
        {
            int temp = storedItem.stackSize;
            storedItem.stackSize = 1; // easy workaround so the new item spawns with stack size of 1
            GameObject newCopyOfObject = storedItem.Deserialize(); // new ItemSerializer().DeserializeData(storedItem);
            newCopyOfObject.transform.position = gameObject.transform.position;
            newCopyOfObject.transform.rotation = gameObject.transform.rotation;
            storedItem.stackSize = temp - 1;

            CraftingMenu.localCraftingMenu.UpdateList();
            return newCopyOfObject;
        }
    }
    public GameObject RemoveItem(int amount)
    {
        if (storedItem == null || storedItem.stackSize < amount)
            return null;

        if (storedItem.stackSize == amount)
        {
            return RemoveItemStack();
        }
        else
        {
            int temp = storedItem.stackSize;
            storedItem.stackSize = amount; // easy workaround so the new item spawns with stack size of amount
            GameObject newCopyOfObject = storedItem.Deserialize(); // new ItemSerializer().DeserializeData(storedItem);
            newCopyOfObject.transform.position = gameObject.transform.position;
            newCopyOfObject.transform.rotation = gameObject.transform.rotation;
            storedItem.stackSize = temp - amount;

            CraftingMenu.localCraftingMenu.UpdateList();
            return newCopyOfObject;
        }
    }

    public void Clear()
    {
        Destroy(miniature);
        miniature = null;

        storedItem = null;
    }
    
    public void SwapItems(SlotInteractions other)
    {
        if (other == null)
            return;

        if (storedItem == null && other.storedItem == null)
            return;
        if (other.storedItem == null)
        {
            other.AddItem(RemoveItemStack());
        }
        else if (storedItem == null)
        {
            AddItem(other.RemoveItemStack());
        }
        else
        {
            GameObject tempItem = RemoveItemStack();
            AddItem(other.RemoveItemStack());
            other.AddItem(tempItem);
        }
    }

    public SlotInteractions FindSlotToSwap()
    {
        if (touchedSlots.Count == 0)
            return null;

        SlotInteractions closest = touchedSlots[0];
        float smallestDistance = Vector3.Distance(transform.position, touchedSlots[0].transform.position);

        for (int i = 1; i < touchedSlots.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, touchedSlots[i].transform.position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closest = touchedSlots[i];
            }
        }

        if (smallestDistance <= minSwapDistance)
        {
            return closest;
        }
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        SlotInteractions otherSlot = other.GetComponent<SlotInteractions>();
        if (otherSlot) // starts touching other slot
        {
            touchedSlots.Add(otherSlot);
        }
    }

    void OnTriggerExit(Collider other)
    {
        SlotInteractions otherSlot = other.GetComponent<SlotInteractions>();
        if (otherSlot) // stops touching other slot
        {
            touchedSlots.Remove(otherSlot);
        }

        if (other.gameObject == Inventory.localInventory.invUI && interactable.mainHand)
        {
            interactable.mainHand.GrabObject(ColliderRelay.FindMainRelay(RemoveItemStack()));
        }
    }

    public void StopHolding()
    {
        if (interactable.mainHand)
            interactable.mainHand.DropObject();
        if (interactable.offHand) // prob not needed because an inv slot cant be held with two hands
            interactable.offHand.DropObject();
    }
}
