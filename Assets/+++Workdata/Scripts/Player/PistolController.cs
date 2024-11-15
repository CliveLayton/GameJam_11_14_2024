using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PistolController : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private float maxGunForce = 10f;
    [SerializeField] private float minGunForce = 1f;
    [SerializeField] private int magazineSize = 10;
    [SerializeField] private float reloadTime = 0.8f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float knockBackForce = 4f;
    [SerializeField] private float bulletKnockBackForce = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private CanvasGroup lostPanel;
    [SerializeField] private TextMeshProUGUI bulletCount;

    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;
    private int currentMagazineSize;
    private bool hasShooted = false;
    private float elapsed = 0f;
    private float hitDistance;
    private float gunForce;
    private GameObject gunParticlesInst;

    private GameInput inputActions;
    private Rigidbody2D rb;
    private MusicManager musicManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new GameInput();
        currentMagazineSize = magazineSize;
    }

    private void Start()
    {
        bulletCount.text = magazineSize.ToString();
        hasShooted = false;
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Update()
    {
        HandleGunRotation();
       
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

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Fire.performed += Shoot;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        
        inputActions.Player.Fire.performed -= Shoot;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            musicManager.PlaySFX(musicManager.hurtSound);
            Vector2 directionToMove = (other.gameObject.transform.position - transform.position).normalized;
            
            rb.AddForce(-directionToMove * knockBackForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            musicManager.PlaySFX(musicManager.hurtSound);
            Vector2 directionToMove = (other.gameObject.transform.position - transform.position).normalized;
            
            rb.AddForce(-directionToMove * bulletKnockBackForce, ForceMode2D.Impulse);
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed && currentMagazineSize > 0 && !hasShooted)
        {
            currentMagazineSize -= 1;
            hasShooted = true;
            musicManager.PlaySFX(musicManager.shootSound);

            LookForObjects();
            
            rb.AddForce(- direction * gunForce, ForceMode2D.Impulse);
            
            bulletCount.text = currentMagazineSize.ToString();
            

            if (currentMagazineSize <= 0)
            {
                musicManager.PlaySFX(musicManager.looseSound);
                lostPanel.ShowCanvasGroup();
                Time.timeScale = 0f;
            }
        }
    }

    private void HandleGunRotation()
    {
        //rotate the gun towards the mouse position
        worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;
        
        //flip the gun when it reaches a 90 degree threshold
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if (angle > 90 || angle < - 90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }

        gun.transform.localScale = localScale;
    }

    private void LookForObjects()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, maxDistance, targetLayer);
        Debug.DrawRay(transform.position, direction * maxDistance, Color.red, 1f);
        
        if (rayHit.collider != null)
        {
            hitDistance = rayHit.distance;
            gunForce = CalculateGunForce(hitDistance);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                Enemy currentEnemy = rayHit.collider.gameObject.GetComponent<Enemy>();
                currentEnemy.Damage(1);
                currentMagazineSize = magazineSize;
            }
        }
        

        if (rayHit.collider == null)
        {
            gunForce = minGunForce;
        }
    }

    private float CalculateGunForce(float distance)
    {
        // Inverse mapping: lower distance results in higher force, and vice versa.
        // Clamp the distance to avoid negative values.
        float normalizedDistance = Mathf.Clamp01(1 - (distance / maxDistance));

        // Interpolate between minForce and maxForce.
        return Mathf.Lerp(minGunForce, maxGunForce, normalizedDistance);
    }
    
}
