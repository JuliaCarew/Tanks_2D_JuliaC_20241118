using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // put this script on the player + enemies, or get references and put it on empty gameobject
    // Health System Variables
    [Header("Health Variables")]
    public int maxHealth = 100;
    private int currentHealth;
    [Header("Damage Variables")]
    public int bulletDamage = 10;
    public int enemyDamage = 10;
    public float bulletHitRadius = 1.0f; // to get hit detection for enemies taking dmg
    public float enemyHitRadius = 1.0f; // to get hit detection for player taking dmg
    public GameObject bulletPrefab;

    public void Awake()
    {
        RunUnitTests();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }
    public string ShowHUD()
    {
        // show all healt system variables as UI text
        return $"Health: {currentHealth}/{maxHealth}";
    }
    /// <summary>
    /// Handles taking damage.
    /// </summary>
    /// <param name="damage">Amount of damage to take.</param>
    public void TakeDamage(int damage)
    {
        //if (currentHealth <= 0) return; // Already dead

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die() // :P 
    {
        Debug.Log($"{gameObject.name} Died :(");
        gameObject.SetActive(false); 
    }

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
                TakeDamage(bulletDamage);
                Destroy(bullet);
                break;
            }
        }
    }
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
