using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public Transform bulletStartPoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        Instantiate(bulletPrefab, bulletStartPoint.position, bulletStartPoint.rotation);
        Debug.Log("Shot bullet");
        //transform the bullet by applying force towards the direction of the bulletstartpoint
        //Destroy(bulletPrefab, 5f);
    }
}
