using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private float destroyTime = 5;
    private ShotInfo shotInfo;

    public void SetShotInfo(ShotInfo shotInfo)
    {
        this.shotInfo = shotInfo;
    }

    public ShotInfo GetShotInfo()
    {
        return shotInfo;
    }

    private void Start ()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
