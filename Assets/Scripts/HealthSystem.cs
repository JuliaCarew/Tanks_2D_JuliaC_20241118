using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    public GameplayUI gameplayUI;

    [Header("Health")]
    public int maxHealth = 30;
    private int currentHealth;

    [Header("Damage")]
    public int bulletDamage = 50;
    public int enemyDamage = 10;
    public int landmineDamage = 10;
    // Radius for detecting if hit
    private float bulletHitRadius = 1.0f; // to get hit detection for enemies taking dmg
    private float enemyHitRadius = 0.5f; // to get hit detection for player taking dmg
    private float landmineHitRadius = 3.0f; // landmine radius detection

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject xpPrefab;
    void Start()
    {
        currentHealth = maxHealth; // TODO: health always starts at 100 ? and bullet's max dmg is 20 ?
    }
    public string ShowHUD() // show all healt system variables as UI text
    {
        return $"Health: {currentHealth}/{maxHealth}";
    }
    // ---------- DAMAGE ---------- //
    public void TakeDamage(int damage) 
    {
        currentHealth -= damage + bulletDamage;
        //Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            Instantiate(xpPrefab, transform.position, Quaternion.identity); // ----- XP!!! ----- //
        }
    }
    // ---------- DIE ---------- //
    private void Die() // :P 
    {
        //Debug.Log($"{gameObject.name} Died :(");
        if (gameObject.CompareTag("Player"))
        {
            gameplayUI.GameOver(); // Trigger GameOver when the player dies
        }
        else
        {
            Destroy(gameObject); // Enemies are destroyed
        }
    }
    // ---------- DETECT BULLET (ENEMY) ---------- //
    // Detects if the object is hit by a bullet using Vectors
    // checks if the bullet is within the hit radius and calls damage
    public void DetectBullet()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach(var bullet in bullets)
        {
            float distance = Vector2.Distance(transform.position, bullet.transform.position);
            if (distance <= bulletHitRadius)
            {
                //Debug.Log($"bullet within {distance} range, taking {bulletDamage} damage");
                TakeDamage(10);
                Destroy(bullet);                
                break;
            }
        }
    }
    // ---------- DETECT ENEMY HIT (PLAYER) ---------- //
    // Detects if the object is hit by an enemy using Vectors
    // checks if the enemy is within the hit radius and calls damage
    public void DetectEnemyHit()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= enemyHitRadius)
            {
                TakeDamage(enemyDamage);
                break;
            }
        }
    } 
    // ---------- DETECT LANDMINE (ENEMY) ---------- //
    public void DetectLandmine() // !!! TEST OUT !!! //
    {
        GameObject[] landmines = GameObject.FindGameObjectsWithTag("landmine");
        foreach (var landmine in landmines)
        {
            float distance = Vector2.Distance(transform.position, landmine.transform.position);
            if (distance <= landmineHitRadius)
            {
                TakeDamage(landmineDamage); // make it so take dmg only once, then destroy landmine
                break;
            }
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
