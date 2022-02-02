using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftableItemDisplay : MonoBehaviour
{
    private CraftableObject displayedObject;

    private int[] ownedComponents;

    [SerializeField] private Text info;
    [SerializeField] private Transform modelParent;
    private GameObject displayedModel;

    private void Update()
    {
        Vector3 lookForward = transform.parent.position - Camera.main.transform.position;

        transform.rotation = Quaternion.LookRotation(lookForward, Vector3.up);
    }

    public void CreateDisplay(CraftableObject craftableObject)
    {
        displayedObject = craftableObject;

        ownedComponents = new int[displayedObject.components.Count];

        if (displayedModel)
            Destroy(displayedModel);
        displayedModel = Instantiate(displayedObject.asset.model, modelParent); //create displayed model
        displayedModel.transform.localPosition = Vector3.zero;

        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < ownedComponents.Length; i++)
        {
            ownedComponents[i] = Inventory.localInventory.GetItemQuantity(displayedObject.components[i].name);
        }

        string infoText = "";
        for (int j = 0; j < displayedObject.components.Capacity; j++)
        {
            infoText += displayedObject.components[j].name + ": " + ownedComponents[j] + " / " + displayedObject.quantity[j] + "\n";
        }
        info.text = infoText;
    }

    public GameObject Craft()
    {
        Debug.Log("Crafting " + displayedObject);
        for(int i=0; i<ownedComponents.Length; i++)
        {
            if (ownedComponents[i] < displayedObject.quantity[i])
                return null;
        }
        for (int i = 0; i < displayedObject.components.Count; i++)
        {
            Destroy(Inventory.localInventory.RemoveItemAmount(displayedObject.components[i].name, displayedObject.quantity[i])); // also updates displayed info
        }

        return Instantiate(displayedObject.asset.prefab, transform.position, Quaternion.identity);
    }
}
