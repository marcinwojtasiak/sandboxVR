using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public interface IUsableObject
{
    void UseClick();
    void UseHold();
    void UseAmount(float value);

    void DpadUp();
    void DpadDown();
    void DpadLeft();
    void DpadRight();
    void MenuButton();

    void TouchpadClick();
    void TouchpadPosition(Vector2 value);
}
