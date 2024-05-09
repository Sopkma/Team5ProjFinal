using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
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
        menu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = !isPaused;
    }
    public void Resume(){
        menu.SetActive(false);
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
