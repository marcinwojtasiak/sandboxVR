using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int numberOfSlots = 20;

    public GameObject storageUI;
    private SlotInteractions[] slots;

    [SerializeField] private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        //TODO: NETWORKING     if(isLocal)
        ConnectToUI();
    }

    public void ChangeStorageState()
    {
        if (isOpen)
            CloseStorage();
        else
            OpenStorage();
    }

    private void OpenStorage()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);
    }

    private void CloseStorage()
    {
        isOpen = false;
        animator.SetBool("isOpen", false);
    }

    private void ConnectToUI()
    {
        slots = new SlotInteractions[numberOfSlots];
        int i = 0;
        foreach (Transform slot in storageUI.transform)
        {
            slots[i] = slot.GetComponent<SlotInteractions>();
            i++;
        }
    }

}