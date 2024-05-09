using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HealthManager : MonoBehaviour
{

    // enemy health
    public float health = 1;
    public int points = 5;
    private ScoreManager scoreManager;
    public GameObject coinPrefab;
    public Game game;
    public GameObject healtUi; //used to get health bar
   
    private Player player;

    // allows for checking if the enemy is defeated once they are hit
    public float Health
    {

        set
        {
            //immidiatly returns health if immunity is turned on for the player. might make enemrys immune for the duration as well?
            if (player.isImmune) { return; }
            print(value);
            health = value;

            if (health <= 0)
            {
                Defeated();
            }
        }
        get { return health; }
    }



    // Start is called before the first frame update
    void Start()
    {
        //groundLayer = LayerMask.GetMask("Ground");
        scoreManager = FindAnyObjectByType<ScoreManager>();
        player = FindAnyObjectByType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("Player"))
        {
            healtUi.GetComponent<HealthbarScript>().updateHealth((int)health, 10);
        }
    }

    // defeated enemy gameobject is deleted
    public void Defeated(){
        scoreManager.addToScore(points);
        print("defeated");
        if (!gameObject.CompareTag("Player"))
        {
            /*
            if (gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = game.GetComponent<Enemy>();
                enemy.ChangeState(EnemyState.DEAD);
            }*/
            Destroy(gameObject); // REMOVE THIS LINE AFTER WE GET FADING TO WORK

            //the below code is responsible for spawning the coins
            //each enemy defeated has a change to spawn 1-3 coins that are then shot out from their location on death
            //this ensures the coins will no longer go out of bounds
            int CoinCount = Random.Range(1, 3);
            for (int i = 0; i < CoinCount; i++) {
                Vector3 spawnPosition = transform.position;
                float ranX = Random.Range(-.5f, .5f);
                float ranY = Random.Range(-.5f, .5f);
                Vector2 force = new Vector2(ranX,ranY);
                var instance = Instantiate(coinPrefab, spawnPosition, transform.rotation);
                instance.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

                //this line is not neccicary becasue we are spawning in coins with the player already assinged to the prefabs, will keep here tho
                //coinPrefab.GetComponent<coinCounter>().player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }
        }
        else
        {
            Player temp = this.GetComponent<Player>();
            temp.speed = 0;
            game.EndGame();
        }
        
    } 
}
