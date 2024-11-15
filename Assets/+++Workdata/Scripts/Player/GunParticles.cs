using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunParticles : MonoBehaviour
{
    [SerializeField] private float destroyTime = 3f;

    private void Start()
    {
        SetDestroyTime();
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }
}
