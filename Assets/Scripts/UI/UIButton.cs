using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    RectTransform rectTransform;

    [SerializeField] private float maxPressDepth;
    private Collider interactionCollider;

    private Vector3 defaultLocalPosition;
    private Vector3 lastPositionOfInteractColl;
    [SerializeField] private bool beingPressed;

    private float snapDistance = 1f;
    private float returnSpeed = 100f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultLocalPosition = rectTransform.anchoredPosition3D;
        Debug.Log(defaultLocalPosition);
        interactionCollider = GameObject.Find("[CameraRig]/Controller (right)/UIInteractor").GetComponent<Collider>(); //zmienic z Find na cos lepszego?
    }


    private void Update()
    {
        float localPosZ = Mathf.Clamp(rectTransform.anchoredPosition3D.z, defaultLocalPosition.z - maxPressDepth, defaultLocalPosition.z);
        rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, localPosZ);
        
        if (!beingPressed)
        {
            //rectTransform.Translate((defaultLocalPosition - rectTransform.anchoredPosition3D).normalized * Time.deltaTime);
            rectTransform.anchoredPosition3D += (defaultLocalPosition - rectTransform.anchoredPosition3D).normalized * Time.deltaTime * returnSpeed;



            if ((defaultLocalPosition - rectTransform.anchoredPosition3D).magnitude < snapDistance)
            {
                rectTransform.anchoredPosition3D = defaultLocalPosition;
            }
        }
        //lastPositionOfInteractColl = interactionCollider.transform.position;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other == interactionCollider)
        {
            beingPressed = true;

            Vector3 positionChange = other.transform.position - lastPositionOfInteractColl;
            Vector3 positionChangeForward = Vector3.Project(positionChange, rectTransform.forward);

            if (Vector3.Dot(rectTransform.forward, positionChangeForward) < 0) // true if position change was in other direction than transform.forward
            {
                float pressDepth = positionChangeForward.magnitude;
                //rectTransform.anchoredPosition3D = new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z - pressDepth);
                rectTransform.anchoredPosition3D += new Vector3(0, 0, -pressDepth);
            }

            
            /*
            float pressDepth = Vector3.Project(positionChange, -transform.forward).z;
            if(pressDepth > 0)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - pressDepth);
            */
        }
    }

    public void OnTriggerExit(Collider other)
    {
        beingPressed = false;
    }
}
