using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour // zrobic zeby dzialalo tylko na uderzanie odpowiednim colliderem (pod odpowiednim kątem?)
{
    [SerializeField] private float maxVelocityThreshold;
    [SerializeField] private float minVelocityThreshold;

    [SerializeField] private float hitsInterval;

    private float intervalTimer = 0f;

    private void Update()
    {
        intervalTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float hitVelocityMagnitude = collision.relativeVelocity.magnitude;

        if(hitVelocityMagnitude >  minVelocityThreshold && intervalTimer < 0f)
        {
            IHittable hittable = collision.gameObject.GetComponent<IHittable>();

            if (hittable != null)
            {
                float intensity;
                if (hitVelocityMagnitude >= maxVelocityThreshold)
                {
                    intensity = 1f;
                }
                else
                {
                    intensity = (hitVelocityMagnitude - minVelocityThreshold) / (maxVelocityThreshold - minVelocityThreshold);
                }
                hittable.Hit(intensity, collision); Debug.Log("Hit intensity" + intensity);

                intervalTimer = hitsInterval;
            }
        }
    }
}
