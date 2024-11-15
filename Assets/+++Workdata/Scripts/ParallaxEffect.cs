using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

    private float startPosX;
    private float startPosY;
    private float lengthX;
    private float lengthY;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        lengthX = GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void FixedUpdate()
    {
        float distanceX = cam.transform.position.x * parallaxEffect;
        float distanceY = cam.transform.position.y * parallaxEffect;
        float movementX = cam.transform.position.x * (1 - parallaxEffect);
        float movementY = cam.transform.position.y * (1 - parallaxEffect);

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        if (movementX > startPosX + lengthX)
        {
            startPosX += lengthX;
        }
        else if (movementX < startPosX - lengthX)
        {
            startPosX -= lengthX;
        }

        if (movementY > startPosY + lengthY)
        {
            startPosY += lengthY;
        }
        else if (movementY < startPosY - lengthY)
        {
            startPosY -= lengthY;
        }
    }
}
