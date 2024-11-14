using System;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform posA, posB;
    [SerializeField] private float moveSpeed;

    private Vector2 targetPos;

    private void Start()
    {
        targetPos = posB.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = (targetPos == (Vector2)posA.position) ? posB.position : posA.position;
            
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
