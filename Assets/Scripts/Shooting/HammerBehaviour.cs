using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBehaviour : MonoBehaviour
{
    public InteractableObject hammerInteractable;
    public Rigidbody hammerRigidbody;
    [HideInInspector] public bool isHammerLocked = false;
    [SerializeField] private float hammerSpeed = 1;
    //[SerializeField] private bool doesBlowback;
    [SerializeField] private Transform backHammerPosition;
    [SerializeField] private float blowbackStrength;

    private Vector3 defaultPosition;

    void Awake()
    {
        defaultPosition = transform.localPosition;
    }
    
    void Update()
    {
        //Debug.Log(PercentageDisplacement());

        if (Input.GetKeyDown(KeyCode.Space))
            Blowback();
        
        if(hammerInteractable.mainHand || hammerInteractable.offHand)
        {
            isHammerLocked = false;
            hammerRigidbody.isKinematic = false;
        }
        else
        {
            hammerRigidbody.isKinematic = true;
        }
    }

    void FixedUpdate()
    {
        if (!hammerInteractable.mainHand && !isHammerLocked)
        {
            ResetPosition();
        }
    }

    void Reset()
    {
        hammerInteractable = GetComponent<InteractableObject>();
        hammerRigidbody = GetComponent<Rigidbody>();
    }

    public float PercentageDisplacement()
    {
        return Vector3.Distance(transform.localPosition, defaultPosition) / backHammerPosition.localPosition.magnitude;
    }

    private void ResetPosition()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, defaultPosition, Time.deltaTime * hammerSpeed);
    }

    public void Blowback()
    {
        hammerInteractable.transform.position = Vector3.MoveTowards(hammerInteractable.transform.position, backHammerPosition.position, Time.deltaTime * blowbackStrength);

        //if(doesBlowback)
        //    hammerInteractable.transform.position = backHammerPosition.position;
        //hammerData.transform.position = Vector3.Project(hammerData.transform.parent.forward * hammerData.maxBoltTranslation, hammerData.transform.parent.forward);
        //Vector3 newPosition = hammerData.transform.localPosition;
        //newPosition.y = newPosition.x = 0;
        //newPosition.z = Mathf.Clamp(newPosition.z, 0, hammerData.maxBoltTranslation);
        //hammerData.transform.localPosition = newPosition;
        //hammerData.transform.Translate(hammerData.positionOffset, Space.Self);
    }
}
