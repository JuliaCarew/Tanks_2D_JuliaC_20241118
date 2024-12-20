using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public EnemyController enemyController;

    [Header("Movement")]
    public float speed =1f;
    public Camera cam;
    
    [Header("Bullets")]
    public Transform bulletStartPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public int bulletLifeTime;
    
    [Header("Landmines")]
    public Transform landmineStartPoint;
    public GameObject landminePrefab;
    public GameObject landmineRadiusVisual;
    private float landmineLifetime = 3f; // how long landmine is active
    private float landmineCooldown = 10f; // can only be placed every 10 seconds?
    private float timer;

    [Header("Rocket")]
    public GameObject rocketPrefab;

    private HealthSystem healthSystem;

    void Start()
    {
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
            if (timer >= landmineCooldown) // disable input?
            {
                return;
            }
            else
            {
                PlaceLandmine();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Input shoot ROCKET");
            Rocket();
        }

        healthSystem.DetectEnemyHit(); // take dmg if hit by enemy
        UpdateAimRotation();
    }

    // ---------- MOVING ---------- //
    // Updates the player's position based on input and rotates the player to face the mouse
    void UpdatePlayerPosition()
    {
        Vector3 playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        transform.position += playerInput.normalized * speed * Time.deltaTime;
    }

    // ---------- AIMING ---------- //
    void UpdateAimRotation()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDirection = mousePos - (Vector2)transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        bulletStartPoint.rotation = Quaternion.Euler(0,0, lookAngle);
    }

    // ---------- PROJECTILES ---------- //
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
    // ---------- LANDMINE ---------- //
    void PlaceLandmine()
    {
        Debug.Log("Placing landmine");
        GameObject landmine = Instantiate(landminePrefab, landmineStartPoint.position, Quaternion.identity);
        
        GameObject radiusVisual = Instantiate(landmineRadiusVisual, landmineStartPoint.position, Quaternion.identity);

        Destroy(radiusVisual, landmineLifetime);
        Destroy(landmine, landmineLifetime);
    }
    // ---------- ROCKET ---------- //
    void Rocket() // shoots with homing logic to track the player
    {
        // Find the nearest enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(bulletStartPoint.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        // Instantiate the rocket
        GameObject rocket = Instantiate(rocketPrefab, bulletStartPoint.position, Quaternion.identity);

        // Set the direction and target for the rocket
        MoveRocket moveRocketref = rocket.GetComponent<MoveRocket>();
        moveRocketref.Initialize(nearestEnemy, bulletSpeed, bulletLifeTime);
    }
}
