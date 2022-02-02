using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleportation : MonoBehaviour
{
    public Transform playArea;
    public Transform laser;
    public Transform player;

    public SteamVR_Action_Boolean teleport;

    private Vector3 lastHit = Vector3.zero;

    void Update()
    {
        if (teleport.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            laser.gameObject.SetActive(true);
            lastHit = Vector3.zero;
        }
        if (teleport.GetState(SteamVR_Input_Sources.LeftHand))
        {
            Ray ray = new Ray(laser.parent.position, laser.forward);
            Physics.Raycast(ray, out RaycastHit hit, 20, LayerMask.GetMask("Terrain")); // Add layer
            if(hit.collider)
            {
                Vector3 newScale = laser.transform.localScale;
                newScale.z = hit.distance;
                laser.transform.localScale = newScale;
                laser.position = Vector3.Lerp(hit.point, laser.parent.position, 0.5f);
                lastHit = hit.point;
            }
            else
            {
                Vector3 newScale = laser.transform.localScale;
                newScale.z = 50;
                laser.transform.localScale = newScale;
                laser.position = laser.parent.position;
                laser.position += laser.forward * 25;
                lastHit = Vector3.zero;
            }
        }
        if (teleport.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            if (lastHit != Vector3.zero)
            {
                playArea.position = lastHit;
                Vector3 translation = playArea.position - player.position;
                translation.y = 0;
                playArea.Translate(translation);
            }
            laser.gameObject.SetActive(false);
        }
    }
}
