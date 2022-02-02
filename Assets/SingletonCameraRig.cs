using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCameraRig : MonoBehaviour
{
    static SingletonCameraRig current;

    void Start()
    {
        if(current)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
    }
}
