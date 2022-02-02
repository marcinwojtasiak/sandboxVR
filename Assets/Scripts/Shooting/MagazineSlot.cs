using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSlot : MonoBehaviour
{
    [SerializeField] private Magazine.AmmoType acceptedAmmo;
    [SerializeField] private GunBehaviour gun;
    public Magazine attachedMagazine;
    private float timeFromDetach; //Dropped magazines attached instantly before - workaround

    void Update()
    {
        timeFromDetach += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (timeFromDetach < 0.2f)
            return;
        if (attachedMagazine) // If magazine already connected, don't connect other one duh
            return;

        Magazine mag = other.GetComponent<Magazine>();
        if (mag != null)
        {
            AttachMag(mag);
            gun.PlayAttachMagSound();
        }
    }

    public void AttachMag(Magazine mag) //osobna funkcja zeby mozna bylo doczepic magazynek z innego skryptu bez zadnej kolizji
    {
        Debug.Log("att mag");
        if (mag && mag.ammoType == acceptedAmmo)
        {
            //if(attachedMagazine.holdingData.isMainHeldLocally)

            attachedMagazine = mag;
            mag.transform.parent = transform;
            mag.transform.localPosition = Vector3.zero;
            mag.transform.localRotation = Quaternion.identity;
            mag.SetNonInteractable(true);
        }
    }

    public void DetachMag()
    {
        Debug.Log("drop mag");
        if (!attachedMagazine)
            return;
        attachedMagazine.gameObject.transform.parent = null;
        attachedMagazine.SetNonInteractable(false);

        attachedMagazine = null;
        timeFromDetach = 0;
    }
}
