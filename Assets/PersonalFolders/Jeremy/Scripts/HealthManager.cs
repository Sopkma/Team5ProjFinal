using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    // enemy health
    public float health = 1;
    public int points = 5;
    private ScoreManager scoreManager;
    public GameObject coinPrefab;
    public Game game;
    public GameObject healtUi; //used to get health bar

    // allows for checking if the enemy is defeated once they are hit
    public float Health
    {
        set
        {
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

        if (!gameObject.CompareTag("Player"))
        {
            /*
            if (gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = game.GetComponent<Enemy>();
                enemy.ChangeState(EnemyState.DEAD);
            }*/
            Destroy(gameObject); // REMOVE THIS LINE AFTER WE GET FADING TO WORK
            int CoinCount = Random.Range(1, 3);
            for (int i = 0; i < CoinCount; i++)
            {
                Vector3 position = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                Instantiate(coinPrefab, transform.position + position, transform.rotation);
                coinPrefab.GetComponent<coinCounter>().player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
