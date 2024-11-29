using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveling : MonoBehaviour
{
    //public HealthSystem healthSystem;
    // XP Orbs
    private float xpPickUpRadius = 3.0f; 
    private float magnetizeSpeed = 4.0f;
    public GameObject xpPrefab;
    // Level UP
    private int currentXP = 0;
    private int level = 1;
    private int levelMax = 25;

    private HashSet<GameObject> orbsPickedUp = new HashSet<GameObject>();
    private List<Coroutine> activeMagnetizeCoroutines = new List<Coroutine>(); 
    void Update()
    {
        PickUpXP();
        LevelUp();
    }
    void PickUpXP() // get magnetize ?
    {
        GameObject[] xpOrbs = GameObject.FindGameObjectsWithTag("XP");
        foreach (var xp in xpOrbs)
        {
            if (orbsPickedUp.Contains(xp)) continue;

            float distance = Vector2.Distance(transform.position, xp.transform.position);
            if (distance <= xpPickUpRadius)
            {
                orbsPickedUp.Add(xp);
                Coroutine magnetizeCoroutine = StartCoroutine(MagnetizeXP(xp));
                //Debug.Log($"XP orb within {distance} range, picking up");
                activeMagnetizeCoroutines.Add(magnetizeCoroutine);    
                break;
            }
        }
        
    }
    IEnumerator MagnetizeXP(GameObject xp) // Move the XP orb towards the player, not counting one by one !!!
    {     
        while (xp != null && Vector2.Distance(transform.position, xp.transform.position) > 0.5f)
        {
            xp.transform.position = Vector2.MoveTowards(
                xp.transform.position,
                transform.position,
                magnetizeSpeed * Time.deltaTime
            );
            yield return null; // Wait for the next frame
        }

        if (xp != null)
        {
            Destroy(xp); // Destroy the XP orb
            currentXP++;  // Increase current XP by 1
            Debug.Log($"Current XP: {currentXP}");
        }
    }
    void LevelUp()
    {
        if (currentXP >= levelMax)
        {
            level++; 
            Debug.Log($"XP: {currentXP} is enough to level up, Leveling up to {level} !");

            levelMax = Mathf.RoundToInt(25 + Mathf.Pow(5 * (level * 0.2f), 2));
            //Debug.Log($"New LevelMax: {levelMax}");

            currentXP = 0; // Reset current XP once level up
        }
    }
    public void ResetLevel()
    {
        foreach (var coroutine in activeMagnetizeCoroutines)
        {
            StopCoroutine(coroutine);
        }
        activeMagnetizeCoroutines.Clear();

        orbsPickedUp.Clear();
        GameObject[] xpOrbs = GameObject.FindGameObjectsWithTag("XP");
        foreach (var xp in xpOrbs)
        {
            Destroy(xp);
        }

        currentXP = 0;
        level = 1;
        levelMax = 25; 
        Debug.Log("Level reset: All XP and orbs cleared, level set to 1");
    }
}