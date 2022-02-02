using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;

public class GunBehaviour : NetworkBehaviour, IUsableObject
{
    public InteractableObject interactable;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject visualBulletPrefab;
    [SerializeField] private GameObject instanitatePoint;
    [SerializeField] private Recoil recoil;

    [SerializeField] private float fireRate;
    private float fireCD;

    [SerializeField] private bool safety; // mozna wywalic serialize fieldy
    [SerializeField] private bool reloaded;
    [SerializeField] private bool bulletInChamber;
    [SerializeField] private bool slidePulled;
    public InteractableObject hammer;
    public HammerBehaviour hammerBehaviour;
    public MagazineSlot magazineSlot;

    [SerializeField] private float damage;
    [SerializeField] private float shootForce;
    [SerializeField] private bool isAutomatic;

    public AudioClip shotSound;
    public AudioClip magDropSound;
    public AudioClip magAttachSound;
    public AudioClip hammerPullSound;
    public AudioClip hammerBackSound;
    public AudioClip noAmmoClickSound;
    public AudioSource soundSource;

    void Reset()
    {
        interactable = GetComponent<InteractableObject>();
    }

    void Update()
    {
        if (!hasAuthority)
            return;

        fireCD -= Time.deltaTime;

        HammerSlideing(hammerBehaviour.PercentageDisplacement());
    }

    [ClientRpc]
    private void RpcVisualShoot(ShotInfo shotInfo, uint ignoredId)
    {
        if(PlayerStats.current.netId != ignoredId)
            ShootBullet(shotInfo, true);
    }

    [Command]
    private void CmdHit(NetworkIdentity hitNetworkIdentity, Vector3 hitPoint)
    {
        IDamagable damagableObject = hitNetworkIdentity.GetComponent<IDamagable>();
        if (damagableObject != null)
        {
            damagableObject.Damage(damage);
        }
    }

    [Command]
    private void CmdVisualShoot(ShotInfo shotInfo, uint commanderId)
    {
        RpcVisualShoot(shotInfo, commanderId);
    }

    public void UpdateHit(Collision collision)
    {
        CmdHit(collision.gameObject.GetComponent<NetworkIdentity>(), collision.GetContact(0).point);
    }


    private void ShootBullet(ShotInfo shotInfo, bool visualOnly)
    {
        GameObject bullet;
        if (visualOnly)
        {
            bullet = Instantiate(visualBulletPrefab, shotInfo.GetShotOrigin(), shotInfo.GetRotation());
        }
        else
        {
            bullet = Instantiate(bulletPrefab, shotInfo.GetShotOrigin(), shotInfo.GetRotation());
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetShotInfo(shotInfo);

        bullet.GetComponent<Rigidbody>().AddForce(shotInfo.GetShotDirection() * shootForce, ForceMode.Impulse);
    }


    private void Shoot()
    {
        fireCD = fireRate;

        ShotInfo shotInfo = new ShotInfo(this, instanitatePoint.transform.position, instanitatePoint.transform.rotation, instanitatePoint.transform.forward, shootForce);
        CmdVisualShoot(shotInfo, PlayerStats.current.netId);
        ShootBullet(shotInfo, false);

        recoil.ApplyRecoil();
        soundSource.PlayOneShot(shotSound);

        if (magazineSlot.attachedMagazine && magazineSlot.attachedMagazine.ammo == 0)
        {
            hammerBehaviour.isHammerLocked = true;
        }

        if (magazineSlot.attachedMagazine != null)
        {
            bulletInChamber = PutBulletToChamber();
        }
        else
        {
            bulletInChamber = false;
        }

        soundSource.PlayOneShot(shotSound);
    }


    protected virtual void TryShoot()
    {
        if (fireCD <= 0f && Ready())
        {
            Shoot();
        }
    }


    protected virtual bool Ready()
    {
        if (safety)
            return false;
        if (!reloaded)
            return false;
        if (bulletInChamber)
            return true;
        DryShot();
        return false;
    }

    protected virtual void DryShot()
    {
        reloaded = false;
        soundSource.PlayOneShot(noAmmoClickSound);
        hammerBehaviour.isHammerLocked = false;
    }

    /* do wyjebania
    public virtual void DropMagazine()
    {
        if (!magazineSlot.attachedMagazine)
            return;
        magazineSlot.attachedMagazine.interactable.SetStatic(false);
        magazineSlot.attachedMagazine.gameObject.transform.parent = null;
        magazineSlot.attachedMagazine.isAttached = false;
        magazineSlot.attachedMagazine.SetAllCollidersStatus(true);

        magazineSlot.attachedMagazine = null;
        magazineSlot.timeFromDetach = 0;
        soundSource.PlayOneShot(magDropSound);

        //rbMag.velocity = controller.velocity;
        //rbMag.angularVelocity = controller.angularVelocity;      TODO mag drop speed
    }*/
    
    public virtual void HammerSlideing(float percent)
    {
        if (percent > 0.95f && slidePulled == false)
        {
            slidePulled = true;
            soundSource.PlayOneShot(hammerPullSound);
        }
        if (percent < 0.05f && slidePulled == true)
        {
            slidePulled = false;
            reloaded = true;
            bulletInChamber = PutBulletToChamber();
            soundSource.PlayOneShot(hammerBackSound);
        }

        /*
        else if (hammerHoldingData.holdingMethod == HoldingData.HoldingMethod.OnlyRotationHandle) // If bolt action
        {
            
            if(percent > 0.95f)
            {
                slidePulled = true;
            }
            if(percent < 0.05f && slidePulled == true && rotation < 0.01)
            {
                slidePulled = false;
                reloaded = true;
            }
            
            
            if (slideBulletTakePercent < percent && !slidePulled)
            {
                slidePulled = true;
                LoadBulletIntoChamber();
            }
            if (slideBulletTakePercent >= percent)
                slidePulled = false;
            if (percent == 1)
                cocked = true;
            
        }*/

    }
    
    private bool PutBulletToChamber()
    {
        if (magazineSlot.attachedMagazine == null)
        {
            return false;
        }
        if (magazineSlot.attachedMagazine.ammo > 0)
        {
            magazineSlot.attachedMagazine.ammo--;
            return true;
        }
        return false;
    }

    public void PlayAttachMagSound()
    {
        soundSource.PlayOneShot(magAttachSound);
    }

    //------------------------------------------------------------------------------------------

    public void UseClick()
    {
        if (!isAutomatic)
        {
            TryShoot();
        }
    }

    public void UseHold()
    {
        if(isAutomatic)
        {
            TryShoot();
        }
    }

    public void UseAmount(float value)
    {
        throw new System.NotImplementedException();
    }

    public void DpadUp()
    {
        throw new System.NotImplementedException();
    }

    public void DpadDown()
    {
        throw new System.NotImplementedException();
    }

    public void DpadLeft()
    {
        throw new System.NotImplementedException();
    }

    public void DpadRight()
    {
        throw new System.NotImplementedException();
    }

    public void MenuButton()
    {
        throw new System.NotImplementedException();
    }

    public void TouchpadClick()
    {
        magazineSlot.DetachMag();
    }

    public void TouchpadPosition(Vector2 value)
    {
        throw new System.NotImplementedException();
    }
}
