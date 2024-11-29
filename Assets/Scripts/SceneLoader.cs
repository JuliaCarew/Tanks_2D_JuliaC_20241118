using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //public GameplayUI gameplayUI;
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void StartGame()
    {
        //gameplayUI.Restart();
        Debug.Log("StartGame");
        SceneManager.LoadScene(1);
    }
}
