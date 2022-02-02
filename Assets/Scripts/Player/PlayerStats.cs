using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour, IDamagable {

    [SyncVar] public float maxHP;
    [SyncVar] public float hp;
    public static PlayerStats current;

    public const float maxHungerAndThirstVal = 100;
    [Range(0, maxHungerAndThirstVal)] public float hunger;
    [Range(0, maxHungerAndThirstVal)] public float thirst;
    private float hungerSpeed = 0.1f;
    private float thirstSpeed = 0.1f;

    private float hungerDamagePerTick = 10;
    private float thirstDamagePerTick = 10;

    private float damageTickDuration = 5;
    private float damageTick;

    void Start()
    {
        if (isLocalPlayer)
        {
            current = this;

        }
        else
        {
            
        }
    }

    void Update()
    {
        damageTick -= Time.deltaTime;

        hunger = Mathf.Clamp(hunger -= hungerSpeed * Time.deltaTime, 0, maxHungerAndThirstVal);
        thirst = Mathf.Clamp(thirst -= thirstSpeed * Time.deltaTime, 0, maxHungerAndThirstVal);

        if (damageTick <= 0)
        {
            if (hunger == 0)
                Damage(hungerDamagePerTick);
            if (thirst == 0)
                Damage(thirstDamagePerTick);

            damageTick = damageTickDuration;
        }
    }

    public void Damage(float damage)
    {
        if (!isServer)
            return;

        hp -= damage;
    }

    [Command]
    public void CmdGrabObject(NetworkIdentity networkIdentity)
    {
        networkIdentity.AssignClientAuthority(connectionToClient);
        networkIdentity.GetComponent<InteractableObject>().RpcSetObjectAsHeld(true);
    }
    [Command]
    public void CmdUnassignObjectAuthority(NetworkIdentity networkIdentity)
    {
        //if (networkIdentity.hasAuthority)
        //{
        networkIdentity.RemoveClientAuthority(connectionToClient);
        //}
        networkIdentity.GetComponent<InteractableObject>().RpcSetObjectAsHeld(false);
     
    }
}
