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
    // Start is called before the first frame update
    void Start(){
        controls = new Controls();
        controls.Enable();
    }

    // Update is called once per frame
    void Update(){
        if (controls.Player.Attack.WasPerformedThisFrame())
        {
            player.Attack();
        }
        if (controls.Player.ResetLevel.WasPerformedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (controls.Player.Interact.WasPerformedThisFrame())
        {
            shop.shopManager();
        }
        if (controls.Player.Dodge.WasPerformedThisFrame()) 
        {
            player.DodgeRoll();
        }
    }

    public void EndGame()
    {
        scoreManager.SendScore("Jay");
    }
}
