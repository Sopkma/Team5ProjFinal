using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public Game gameM;
    public int goTo = 0;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(goTo == 0)
            {
                print("<color=green>GAME WIN</color>");
                gameM.EndGame();
            }
            else
            {
                ScoreManager sm = FindObjectOfType<ScoreManager>();
                sm.StoreScore();
                player.SaveComponents();
                SceneManager.LoadScene(goTo);
            }
        }
    }
}
