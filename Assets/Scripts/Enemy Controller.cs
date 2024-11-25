using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Main variables
    [Header("Main Variables")]
    public GameObject player;
    public float speed = 1f;
    // Spawning variables
    [Header("Spawning Variables")]
    public GameObject enemyPrefab;
    public int initialSpawnCount = 5; 
    public float spawnInterval = 10f; // seconds between waves
    public float difficultyIncreaseRate = 1.2f; // by  __% in decimal
    public float spawnRadius = 10f; // for player radius check

    public int currentSpawnCount; // increments by IncreaseRate at Interval
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
            ChasePlayer();
             // Check for bullet hits using the HealthSystem
            if (healthSystem != null)
            {
                healthSystem.DetectBullet(); 
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
                // because instantiated prefab enemies will have this script w/o player reference
                EnemyController enemyController = enemy.GetComponent<EnemyController>(); 
                if (enemyController != null)
                {
                    enemyController.player = player; // assign the prefab's player object to this one
                }
                Debug.Log("Enemy spawned");
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
        Vector2 direction = (player.transform.position - transform.position).normalized;

        float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angleToPlayer);
    }
}
