using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 8f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask whatDestroysBullet;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        SetDestroyTime();
        
        SetStraightVelocity();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((whatDestroysBullet.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTime);
    }
    
}
