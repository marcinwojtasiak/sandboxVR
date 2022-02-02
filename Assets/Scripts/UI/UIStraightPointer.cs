using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class UIStraightPointer : MonoBehaviour
{
    [SerializeField] private SteamVR_Input_Sources thisHand;
    [SerializeField] private SteamVR_Action_Boolean useAction;

    [SerializeField] private LineRenderer pointerRenderer;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 direction;

    private bool isEnabled; //enable when ui is opened

    private float maxDistance = 100f;


    private void Update()
    {
        if (isEnabled)
        {
            Point();
        }
    }

    private bool CastRay(out RaycastHit raycastHit)
    {
        bool hasHit = Physics.Raycast(startPoint, direction, out raycastHit, maxDistance, LayerMask.NameToLayer("UI"));
        return hasHit;
    }

    private void DrawRenderer(Vector3 hitPoint)
    {
        pointerRenderer.SetPositions(new Vector3[] { startPoint, hitPoint });
    }

    private void Point()
    {
        RaycastHit hit;
        if (CastRay(out hit))
        {
            DrawRenderer(hit.point);
        }
        else
        {
            DrawRenderer(startPoint + direction.normalized * maxDistance);
        }

    }
  

    
}
