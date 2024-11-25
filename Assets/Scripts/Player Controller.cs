using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    [Header("Movement Variables")]
    public float speed =1f;
    public Camera cam;
    // Bullet Variables
    [Header("Bullet Variables")]
    public Transform bulletStartPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public int bulletLifeTime;
    [Header("Landmine Variables")]
    public Transform landmineStartPoint;
    public GameObject landminePrefab;
    public GameObject landmineRadiusVisual;
    public float landmineLifetime = 3f;
    float landmineCooldown = 10f; // can only be placed every 10 seconds
    private float timer;

    private HealthSystem healthSystem;
    void Start()
    {
        landmineRadiusVisual.SetActive(false);
        healthSystem = GetComponent<HealthSystem>();
    }
    void Update()
    {
        UpdatePlayerPosition();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown("space"))
        {
            timer += Time.deltaTime;
            if (timer >= landmineCooldown)
            {
                // disable input
                return;
            }
            else
            {
                PlaceLandmine();
            }
        }

        healthSystem.DetectEnemyHit(); // take dmg if hit by enemy
    }

    // Updates the player's position based on input and rotates the player to face the mouse
    void UpdatePlayerPosition()
    {
        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        transform.position = transform.position + playerInput.normalized * speed * Time.deltaTime;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log($"Got mouse position at  [{mousePos.y}], [{mousePos.x}]");

        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0, lookAngle);
        //Debug.Log($"Rotating Player at {lookAngle}");
    }

    // Instantiates a bullet at the player's shooting point and moves it towards the mouse position
    void Shoot()
    {
        //Debug.Log("Shoot bullet");
        GameObject bullet = Instantiate(bulletPrefab, bulletStartPoint.position, Quaternion.identity); // Intantiate bullet

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get mouse position
        mousePosition.z = 0; // since 2D Z is always 0

        Vector3 shootDirection = (mousePosition - bulletStartPoint.position).normalized; // Calculate shooting direction -

        // Initialize bullets movement with speed & lifetime
        MoveBullet bulletMovement = bullet.GetComponent<MoveBullet>(); 
        bulletMovement.Initialize(shootDirection, bulletSpeed, bulletLifeTime); // Call Initialize() from MoveBullet.cs
    }
    void PlaceLandmine()
    {
        Debug.Log("Placing landmine");
        GameObject landmine = Instantiate(landminePrefab, landmineStartPoint.position, Quaternion.identity);
        //Instantiate(landmineRadiusVisual, landmine.position, Quaternion.identity); 
        //landmineRadiusVisual.SetActive(true); // place radius at teh instantiated landmine prefab

        //timer += Time.deltaTime;

        //Destroy(landminePrefab, landmineLifetime);
    }
}
