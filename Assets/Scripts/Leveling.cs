using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
public class Leveling : MonoBehaviour
{
    // XP Orbs
    private float xpPickUpRadius = 3.0f; 
    private float magnetizeSpeed = 4.0f;
    public GameObject xpPrefab;

    // UI elements
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;
    
    // Level UP
    private int currentXP = 0;
    public int level = 1;
    private int levelMax = 25;

    // for xp magnietize coroutine
    private HashSet<GameObject> orbsPickedUp = new HashSet<GameObject>(); // fixes the issue of one xp orb counting for all nearby orbs when picked up
    private List<Coroutine> activeMagnetizeCoroutines = new List<Coroutine>(); 
    
    void Start()
    {
        UpdateUI(); 
    }
    void Update()
    {
        PickUpXP();
        LevelUp();
    }
    
    // ---------- XP PICKUP ---------- //
    void PickUpXP() // find XP obj, pick up if within radius, magnetize to player
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

    // ---------- MAGNETIZE XP COROUTINE ---------- //
    IEnumerator MagnetizeXP(GameObject xp) // Move the XP orb towards the player, not counting one by one !!!
    {     
        while (xp != null && Vector2.Distance(transform.position, xp.transform.position) > 0.5f)
        {
            xp.transform.position = Vector2.MoveTowards(
                xp.transform.position,
                transform.position,
                magnetizeSpeed * Time.deltaTime
            );
            yield return null; // wait for the next frame
        }

        if (xp != null)
        {
            Destroy(xp); // destroy XP orb
            currentXP++;  // increase current XP
            UpdateUI();
            Debug.Log($"Current XP: {currentXP}");
        }
    }

    // ---------- LEVEL UP ---------- //
    void LevelUp() // using a level curve equation to increment XP needed for each level
    {
        if (currentXP >= levelMax)
        {
            level++; 
            Debug.Log($"XP: {currentXP} is enough to level up, Leveling up to {level} !");

            levelMax = Mathf.RoundToInt(25 + Mathf.Pow(5 * (level * 0.2f), 2));
            //Debug.Log($"New LevelMax: {levelMax}");

            currentXP = 0; // reset current XP once level up
            UpdateUI();
        }
    }

    // ---------- RESET LVL ---------- //
    public void ResetLevel()
    {
        foreach (var coroutine in activeMagnetizeCoroutines)
        {
            if (coroutine != null) // check if coroutine is not null
            {
                StopCoroutine(coroutine);
            }
        }
        activeMagnetizeCoroutines.Clear(); // stop coroutine

        orbsPickedUp.Clear(); // clear hashset
        GameObject[] xpOrbs = GameObject.FindGameObjectsWithTag("XP");
        foreach (var xp in xpOrbs)
        {
            Destroy(xp);
        }

        currentXP = 0;
        level = 1;
        levelMax = 25; 
        UpdateUI();
        Debug.Log("Level reset: All XP and orbs cleared, level set to 1");
    }

    // ---------- UPDATE UI ---------- //
    public void UpdateUI() // the eggs: UI for XP and lvl
    {
        if (levelText != null) levelText.text = "Level: " + level;
        if (xpText != null) xpText.text = "" + currentXP + " / " + levelMax;
    }
}