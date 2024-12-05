using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    // sets up bullet vartiables to be used in ShootBullet.cs - Shoot()
    // this handles moving the bullets when shot, using direction, speed, lifetime
    private Vector3 direction; 
    private float speed; 
    private float lifeTime; 
    private float timer; // timer to track how long the bullet has been active

    public void Initialize(Vector3 shootDirection, float bulletSpeed, float bulletLifeTime)
    {
        //Debug.Log("Bullet movement initialized");
        direction = shootDirection; 
        speed = bulletSpeed; 
        lifeTime = bulletLifeTime; 
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        // increment the timer and destroy the bullet if it exceeds its lifetime
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
