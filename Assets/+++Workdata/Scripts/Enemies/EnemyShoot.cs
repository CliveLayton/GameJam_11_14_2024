using System;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float reloadTime = 0.8f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Quaternion bulletRotation;

    private GameObject bulletInst;
    private float elapsed = 0f;
    private bool hasShooted = false;
    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandleShooting();
        
        if (hasShooted)
        {
            elapsed += Time.deltaTime;
            
            if (elapsed > reloadTime)
            {
                hasShooted = false;
                elapsed = 0f;
            }
        }
    }

    private void HandleShooting()
    {
        if (!hasShooted && col.enabled)
        {
            bulletInst = Instantiate(bullet, bulletSpawnPoint.position, bulletRotation);
            
            hasShooted = true;
        }
    }
}
