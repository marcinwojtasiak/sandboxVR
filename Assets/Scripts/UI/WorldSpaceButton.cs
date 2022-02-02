using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class WorldSpaceButton : MonoBehaviour
{
    [SerializeField] private float maxPressDepth;
    private Collider interactionCollider;

    private Vector3 defaultLocalPosition;
    private Vector3 lastPositionOfInteractColl;
    public bool beingPressed;
    private bool activated;

    private float snapDistance = 0.1f; //percentage
    private float activationTolerance = 0.1f; //percentage

    [SerializeField] private UnityEngine.Events.UnityEvent onClickEvent;

    private void Awake()
    {
        defaultLocalPosition = transform.localPosition;
        interactionCollider = GameObject.Find("[CameraRig]/Controller (right)/UIInteractor").GetComponent<Collider>(); //zmienic z Find na cos lepszego?       
    }

    private void Update()
    {
        float localPosZ = Mathf.Clamp(transform.localPosition.z, defaultLocalPosition.z, defaultLocalPosition.z + maxPressDepth);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, localPosZ);

        if(Mathf.Abs(localPosZ - (defaultLocalPosition.z + maxPressDepth)) / maxPressDepth < activationTolerance && !activated)
        {
            if (onClickEvent != null)
            {
                onClickEvent.Invoke();
                activated = true;
            }
            else
            {
                Debug.LogWarning("World Space Button on " + gameObject.name + " has no event to invoke.");
            }
        }

        if (!beingPressed)
        {
            transform.Translate((defaultLocalPosition - transform.localPosition).normalized * Time.deltaTime);


            if ((defaultLocalPosition - transform.localPosition).magnitude / maxPressDepth < snapDistance)
            {
                transform.localPosition = defaultLocalPosition;
                activated = false;
            }
        }
        lastPositionOfInteractColl = interactionCollider.transform.position;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other == interactionCollider)
        {
            beingPressed = true;

            Vector3 positionChange = other.transform.position - lastPositionOfInteractColl;
            Vector3 positionChangeForward = Vector3.Project(positionChange, transform.forward);

            if (Vector3.Dot(transform.forward, positionChangeForward) > 0) // true if position change was in the same direction as transform.forward
            {
                float pressDepth = positionChangeForward.magnitude;
                transform.position += transform.forward * pressDepth;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        beingPressed = false;
    }
}
