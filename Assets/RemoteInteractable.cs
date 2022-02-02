using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RemoteInteractable : MonoBehaviour, IUsableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TouchpadClick()
    {
        Debug.Log("guzik");
    }

    public void DpadDown() { }
    public void DpadLeft() { }
    public void DpadRight() { }
    public void DpadUp() { }
    public void MenuButton() { }
    public void TouchpadPosition(Vector2 value) { }
    public void UseAmount(float value) { }
    public void UseClick() { }
    public void UseHold() { }
}
