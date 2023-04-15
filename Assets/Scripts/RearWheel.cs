using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearWheel : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem smokeParticleSystem;

    private Quaternion lastRotation;

    void Start()
    {
        lastRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        //var emission = smokeParticleSystem.emission;

        //Debug.Log(Quaternion.Angle(transform.rotation, lastRotation));
        
        //if (Quaternion.Angle(transform.rotation, lastRotation) > 1f)
        //{
        //    emission.rateOverTime = 60f;
        //}
        //else
        //{
        //    emission.rateOverTime = 0f;
        //}

        //lastRotation = transform.rotation;
    }
}
