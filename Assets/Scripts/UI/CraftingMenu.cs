using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
    public static CraftingMenu localCraftingMenu;

    [SerializeField] private List<CraftableObject> craftableItems = new List<CraftableObject>();
    [SerializeField] private CraftableItemDisplay craftableItemDisplay;
    private List<CraftableItemDisplay> itemDisplays = new List<CraftableItemDisplay>();

    private const float baseRadius = 0.1f;
    private const float radiusConst = 0.01f;

    private bool isOpen = false;

    private void Start()
    {
        localCraftingMenu = this;
        CreateList();
        gameObject.SetActive(false);
    }

    private void CreateList()
    {
        float radius = baseRadius + craftableItems.Count * radiusConst;
        for(int i = 0; i < craftableItems.Count; i++)
        {            
            float angle = i * Mathf.PI * 2f / craftableItems.Count;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius); //calculate position for the display panel

            GameObject displayObject = Instantiate(craftableItemDisplay.gameObject, transform); //create the display panel
            displayObject.transform.localPosition = pos;

            CraftableItemDisplay display = displayObject.GetComponent<CraftableItemDisplay>();
            itemDisplays.Add(display);
            display.CreateDisplay(craftableItems[i]);
        }
    }

    public void UpdateList()
    {
        foreach(CraftableItemDisplay display in itemDisplays)
        {
            display.UpdateDisplay();
        }
    }

    public void ChangeMenuState()
    {
        if (!isOpen) // then open
        {
            gameObject.SetActive(true); // add some kind of transition
            UpdateList();
            isOpen = true;
        }
        else // then close
        {
            gameObject.SetActive(false);
            isOpen = false;
        }
    }
}
