using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Main Variables")]
    public GameObject player;
    public Transform enemyAim;
    private float speed = 1f;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public GameObject enemyAimPrefab;

    [Header("Spawning")]
    private int initialSpawnCount = 5; 
    private float spawnInterval = 10f; // seconds between waves
    private float difficultyIncreaseRate = 1.15f; // by  __% in decimal
    private float spawnRadius = 10f; // for player radius check
    private int currentSpawnCount; // increments by IncreaseRate at Interval

    private HealthSystem healthSystem;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (gameObject.CompareTag("Spawner")) // need spawner to always be in the scene, not like inst. enemy prefab
        {
            currentSpawnCount = initialSpawnCount;
            StartCoroutine(SpawnEnemyWaves());
        }
        else // If this is an enemy, initialize its HealthSystem
        {
            healthSystem = GetComponent<HealthSystem>();
        }
    }
    void Update()
    {
        if (!gameObject.CompareTag("Spawner")) // if it's an enemy
        {
            if (player != null)
            {
                ChasePlayer();
            }
            // Check for bullet/landmine hits using the HealthSystem
            if (healthSystem != null)
            {
                healthSystem.DetectBullet(); 
                healthSystem.DetectLandmine();
                healthSystem.DetectRocket();
            }
        }
    }
    
    // ---------- SPAWN ENEMY WAVES ---------- //
    // Coroutine to spawn enemies around the player at regular intervals
    IEnumerator SpawnEnemyWaves()
    {
        while (true)
        {
            for (int i =0; i < currentSpawnCount; i++)
            {
                Vector3 spawnPosition = GetRandomOutsideRadius();
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                
                // instantiate enemy aim arrow & parent it so follows position while also rotating alone
                GameObject aimObject = Instantiate(enemyAimPrefab, spawnPosition, Quaternion.identity);
                aimObject.transform.parent = enemy.transform;
                aimObject.transform.localPosition = Vector3.zero;
                
                // because instantiated prefab enemies will have this script w/o player reference
                EnemyController enemyController = enemy.GetComponent<EnemyController>(); 
                if (enemyController != null)
                {
                    enemyController.player = player; // assign the prefab's player object to this one
                    enemyController.enemyAim = aimObject.transform;
                }
                //Debug.Log("Enemy spawned");
            }
            // increment enemies spawned by difficulty multiplier
            currentSpawnCount = Mathf.CeilToInt(currentSpawnCount * difficultyIncreaseRate); 
            yield return new WaitForSeconds(spawnInterval); // coroutine method to spawn only at set interval
        }
    }

    // ---------- RND SPAWN IN RADIUS ---------- //
    // Calculates a random spawn position outside a radius around the player
    private Vector3 GetRandomOutsideRadius()
    {
        if (player == null)
        {
            return Vector3.zero;
        }

        float angle = Random.Range(0f, 360f); // circle radius angles
        float distance = Random.Range(spawnRadius, spawnRadius + 5f); // distance in the radius

        float x = player.transform.position.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = player.transform.position.y + distance * Mathf.Sin(angle * Mathf.Deg2Rad); // getting btw -1 - 1 on X and Y

        return new Vector3(x,y,0);
    }

    // ---------- ENEMY CHASE ---------- //
    // Makes the enemy chase the player by moving and rotating towards them
    private void ChasePlayer()
    {
        if (player == null)
        {
            return;
        }

        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (enemyAim != null)
        {
            Vector2 aimDirection = (player.transform.position - enemyAim.position).normalized;
            float angleToPlayer = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            enemyAim.rotation = Quaternion.Euler(0, 0, angleToPlayer);
        }  
    }

    // ---------- RESET ALL ---------- //
    public void ResetSpawns()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
        currentSpawnCount = initialSpawnCount;
    }
}
