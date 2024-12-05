using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    public HealthSystem healthSystem;
    public EnemyController enemySpawner;
    public Leveling levelingref;

    [Header("Ref to Player")]
    public GameObject player;
    private Vector3 playerStartPosition;
    
    [Header("UI Screens")]
    public GameObject gameOverScreen;
    public GameObject onScreenWinCondition;
    public float winConditionDuration = 3f;
    public GameObject winScreen;
    public int winLevel = 5; // change to win at diff levels
    void Start()
    {
        if (onScreenWinCondition != null) // win condition needs to disappear at set time, so coroutine
        {
            onScreenWinCondition.SetActive(true);
            StartCoroutine(HideWinConditionAfterDelay());
        }

        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);

        if (player != null)
            playerStartPosition = player.transform.position;
    }

    void Update() // check level to update winscreen
    {
        if (levelingref.level >= winLevel)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // ---------- RESTART GAME ---------- //
    public void Restart()
    {
        Debug.Log("Restart game");
        Time.timeScale = 1;

        if (levelingref != null) // reset level
        {
            levelingref.ResetLevel(); 
        }

        if (healthSystem != null) // reset health
        {
            healthSystem.ResetHealth(); 
        }

        if (player != null) // re-position & respawn player
        {
            player.transform.position = playerStartPosition;
            player.SetActive(true);
        }
       
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies) // destroy all enemies
        {
            Destroy(enemy);
        }

        if (enemySpawner != null) // reset enemy spawns
        {
            enemySpawner.ResetSpawns(); 
        }
        
        gameOverScreen.SetActive(false); // hide the game over screen last
    }

    // ---------- QUIT TO MENU ---------- //
    public void QuitToMenu()
    {
        Debug.Log("Quit to menu");
        SceneManager.LoadScene(0);
    }

    // ---------- GAME OVER ---------- //
    public void GameOver() 
    {
        Debug.Log("Game Over");
        gameOverScreen.SetActive(true);
        levelingref.ResetLevel();
        Time.timeScale = 0;
    }

    // ---------- WIN CONDITION ---------- //
    private IEnumerator HideWinConditionAfterDelay()
    {
        yield return new WaitForSeconds(winConditionDuration);

        if (onScreenWinCondition != null)
        {
            onScreenWinCondition.SetActive(false);
        }
    }
}
