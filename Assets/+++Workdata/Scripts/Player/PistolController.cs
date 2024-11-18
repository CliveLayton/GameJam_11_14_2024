using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class PistolController : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private float maxGunForce = 10f;
    [SerializeField] private float minGunForce = 1f;
    [SerializeField] private float enemyGunForce = 1f;
    [SerializeField] private int magazineSize = 10;
    [SerializeField] private float reloadTime = 0.8f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float knockBackForce = 4f;
    [SerializeField] private float bulletKnockBackForce = 10f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private CanvasGroup lostPanel;
    [SerializeField] private TextMeshProUGUI bulletCount;
    [SerializeField] private Transform gunParticlePoint;
    [SerializeField] private GameObject gunParticlesPrefab;
    [SerializeField] private CinemachineVirtualCamera cinemachineCam;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;
    [SerializeField] private Volume volume;
    [SerializeField] private float blinkSpeed;


    private Vignette vignette;
    private bool doEffect = false;
    private bool isIncreasing = true;
    private float vignetteIntensity = 0f;
    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;
    private int currentMagazineSize;
    private bool hasShooted = false;
    private float elapsed = 0f;
    private float hitDistance;
    private float gunForce;
    private GameObject gunParticlesInst;
    private float shakeTimer;
    private float startingIntensity;
    private float shakeTimerTotal;
    
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

        // Get the Vignette effect from the Volume Profile
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true; // Enable override
        }
    }

    private void Update()
    { 
        HandleGunRotation();

        if (doEffect)
        {
            if (vignette != null)
            {
                if (isIncreasing)
                {
                    vignetteIntensity += Time.deltaTime * blinkSpeed;
                    if (vignetteIntensity >= 0.35f)
                    {
                        isIncreasing = false;
                    }
                }
                else
                {
                    vignetteIntensity -= Time.deltaTime * blinkSpeed;
                    if (vignetteIntensity <= 0f)
                    {
                        isIncreasing = true;
                    }
                }

                vignette.intensity.value = vignetteIntensity;
            }
        }

        if (hasShooted)
       {
           if (shakeTimer > 0)
           {
               shakeTimer -= Time.deltaTime;
               if (shakeTimer <= 0f)
               {
                   CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                       cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                   cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                       Mathf.Lerp(startingIntensity, 0f, 1- (shakeTimer / shakeTimerTotal));
               }
           }
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
            //ShakeCamera(shakeIntensity, shakeTime);
            musicManager.PlaySFX(musicManager.shootSound);
            GameObject particles = Instantiate(gunParticlesPrefab, gunParticlePoint.position, gun.transform.rotation);
            particles.transform.SetParent(null);
            Destroy(particles, 2f);

            LookForObjects();
            
            rb.AddForce(- direction * gunForce, ForceMode2D.Impulse);
            
            bulletCount.text = currentMagazineSize.ToString();
            

            if (currentMagazineSize <= 0)
            {
                musicManager.PlaySFX(musicManager.looseSound);
                lostPanel.ShowCanvasGroup();
                Time.timeScale = 0f;
            }

            if (currentMagazineSize <= 2)
            {
                doEffect = true;
            }
            else
            {
                doEffect = false;
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
                gunForce = enemyGunForce;
            }
        }
        

        if (rayHit.collider == null)
        {
            gunForce = minGunForce;
        }
    }

    private void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
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
