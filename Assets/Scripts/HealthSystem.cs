using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // put this script on the player + enemies, or get references and put it on empty gameobject
    // Health System Variables
    [Header("Health Variables")]
    //HealthSystem healthSystem = new HealthSystem();
    public int maxHealth = 30;
    private int currentHealth;
    [Header("Damage Variables")]
    public int bulletDamage = 50;
    public int enemyDamage = 10;
    public int landmineDamage = 10;
    // Radius for detecting if hit
    public float bulletHitRadius = 1.0f; // to get hit detection for enemies taking dmg
    public float enemyHitRadius = 1.0f; // to get hit detection for player taking dmg
    public float landmineHitRadius = 5f; // landmine radius detection

    public GameObject bulletPrefab;
    public GameObject xpPrefab;

    public void Awake()
    {
        //RunUnitTests();
    }
    void Start()
    {
        currentHealth = maxHealth; // TODO: health always starts at 100 ? and bullet's max dmg is 20 ?
    }
    public string ShowHUD()
    {
        // show all healt system variables as UI text
        return $"Health: {currentHealth}/{maxHealth}";
    }
    // ---------- DAMAGE ---------- //
    public void TakeDamage(int damage) 
    {
        currentHealth -= damage + bulletDamage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            Instantiate(xpPrefab, transform.position, Quaternion.identity); // ----- XP!!! ----- //
        }
    }
    // ---------- DIE ---------- //
    private void Die() // :P 
    {
        Debug.Log($"{gameObject.name} Died :(");
        gameObject.SetActive(false); 
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
                Debug.Log($"bullet within {distance} range, taking {bulletDamage} damage");
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
    public static void RunUnitTests()
    {
        Test_TakeDamage();
    }
    public static void Test_TakeDamage()
    {
        HealthSystem system = new HealthSystem();
        system.TakeDamage(10);
        Debug.Assert(90 == system.currentHealth, " TEST TakeDamage_HealthOnly Failed");
    }
}
