using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [HideInInspector] public Controls controls;
    public Player player;
    public playerShop shop;
    public ScoreManager scoreManager;
    public GameObject UI;
    public GameObject FinalScoreUI;
    public GameObject GameOverUI;

    // Start is called before the first frame update
    void Start(){
        UI.SetActive(true);
        controls = new Controls();
        controls.Enable();
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update(){
        if (controls.Player.Attack.WasPerformedThisFrame())
        {
            player.Attack();
        }
        if (controls.Player.ResetLevel.WasPerformedThisFrame())
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (controls.Player.Interact.WasPerformedThisFrame())
        {
            shop.shopManager();
        }
        if (controls.Player.Dodge.WasPerformedThisFrame()) 
        {
            //player.HandleDash();
        }
    }

    public void EndGame()
    {
        // scoreManager.SendScore("Jay");
        FinalScoreUI.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void GameOver()
    {
        // scoreManager.SendScore("Jay");
        GameOverUI.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
