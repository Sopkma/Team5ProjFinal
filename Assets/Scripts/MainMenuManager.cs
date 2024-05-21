using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("coins", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
