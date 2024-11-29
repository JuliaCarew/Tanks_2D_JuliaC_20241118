using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    public HealthSystem healthSystem;
    public EnemyController enemySpawner;

    [Header("Ref to Player")]
    public GameObject player;
    private Vector3 playerStartPosition;
    
    [Header("UI Screens")]
    public GameObject gameOverScreen;
    public GameObject tutorialScreen;
    void Start()
    {
        gameOverScreen.SetActive(false);
        tutorialScreen.SetActive(false);

        if (player != null)
            playerStartPosition = player.transform.position;
    }
    public void Restart()
    {
        Debug.Log("Restart game");
        // Hide the game over screen and resume the game
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;

        Leveling leveling = player.GetComponent<Leveling>();
        if (leveling != null)
        {
            leveling.ResetLevel(); 
        }

        if (healthSystem != null)
        {
            healthSystem.ResetHealth(); 
        }

        // Reset the player's position and re-enable it
        if (player != null)
        {
            player.transform.position = playerStartPosition;
            player.SetActive(true);
        }

        // Reset enemy spawns
        if (enemySpawner != null)
        {
            enemySpawner.ResetSpawns(); 
        }
    }
    public void QuitToMenu()
    {
        Debug.Log("Quit to menu");
        SceneManager.LoadScene(0);
    }
    public void TutorialScreen()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("tutorial screen up");
            tutorialScreen.SetActive(true);
        }
    }
    public void GameOver() // not triggering
    {
        Debug.Log("Game Over");
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
