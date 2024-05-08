using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent (typeof(AudioSource))]
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
    public AudioClip damageSound;
    private AudioSource audioSource;

    // allows for checking if the enemy is defeated once they are hit
    public float Health
    {

        set
        {
            //immidiatly returns health if immunity is turned on for the player. might make enemrys immune for the duration as well?
            if (player.isImmune) { return; }
            print(value);
            if (health > value)
            {
                audioSource.PlayOneShot(damageSound);
            }
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
        audioSource = GetComponent<AudioSource>();
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
            
            if (gameObject.CompareTag("Enemy"))
            {
                Collider2D eColl = gameObject.GetComponent<Collider2D>();
                eColl.enabled = false;
                Enemy enemy = gameObject.GetComponent<Enemy>();
                enemy.ChangeState(EnemyState.DEAD);
            }
            // Destroy(gameObject, 1f); // REMOVE THIS LINE AFTER WE GET FADING TO WORK

            int CoinCount = Random.Range(1, 3);
            for (int i = 0; i < CoinCount; i++) {
                //new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
                Vector3 spawnPosition = transform.position;
                
                //picks random range around object
                spawnPosition.x += Random.Range(-1, 1);
                spawnPosition.y += Random.Range(-1, 1);

                //clamps the spawns by the rooms bounds
                spawnPosition.x = Mathf.Clamp(spawnPosition.x, player.spawningBounds.min.x, player.spawningBounds.max.x);
                spawnPosition.y = Mathf.Clamp(spawnPosition.y, player.spawningBounds.min.y, player.spawningBounds.max.y);

                //spawns object
                Instantiate(coinPrefab, spawnPosition, transform.rotation);
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
