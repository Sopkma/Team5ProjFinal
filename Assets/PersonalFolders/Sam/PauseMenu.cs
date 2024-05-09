using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //  to add the pause screen to a scene, add this script to the game manager and connect the Pause Screen (not the pause menu) to its PauseScreen member
    public GameObject PauseScreen;
    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        PauseScreen.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused){
                Pause();
            } else {
                Resume();
            }
        }
    }

    public void Pause(){
        PauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = !isPaused;
    }
    public void Resume(){
        PauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = !isPaused;
        
    }
    public void Quit(){
        Application.Quit();
    }
    
    public void Main(){
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
