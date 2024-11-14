using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float respawnTime = 30f;

    private float currentHealth;
    private BoxCollider2D col;
    private SpriteRenderer sr;
    private float elapsed = 0f;
    private bool isRespawning;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isRespawning)
        {
            elapsed += Time.deltaTime;
            
            if (elapsed > respawnTime)
            {
                RespawnEnemy();
                elapsed = 0f;
            }
        }
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            DespawnEnemy();
        }
    }

    private void DespawnEnemy()
    {
        sr.enabled = false;
        col.enabled = false;
        isRespawning = true;
    }

    private void RespawnEnemy()
    {
        sr.enabled = true;
        col.enabled = true;
        isRespawning = false;
    }
}
