using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject swordAndSpear;

    [HideInInspector]
    public float maxHealth;

    // allows for checking if the enemy is defeated once they are hit
    public float Health
    {

        set
        {
            //immidiatly returns health if immunity is turned on for the player. might make enemrys immune for the duration as well?
            if (health > value) { audioSource.PlayOneShot(damageSound); }
            if ((player.isImmune || player.immuityFromDamage) && CompareTag("Player")) { return; }
            //print(value);
            
            health = value;
            if (this.gameObject.CompareTag("Boss"))
            {
                BossType1 boss = GetComponent<BossType1>();
                boss.SetHealthBar(health/maxHealth);
            }

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
        maxHealth = health;
        audioSource = GetComponent<AudioSource>();
        //groundLayer = LayerMask.GetMask("Ground");
        scoreManager = FindAnyObjectByType<ScoreManager>();
        player = FindAnyObjectByType<Player>();
        player.transform.GetChild(1).GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime; // TODO make death changes on main player model
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
    public void Defeated() {
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

            if (gameObject.CompareTag("Boss"))
            {
                Collider2D eColl = gameObject.GetComponent<Collider2D>();
                eColl.enabled = false;
                BossType1 boss = gameObject.GetComponent<BossType1>();
                boss.ChangeState(MinotaurState.DEFEATED);

                spawnCoins(25);



            }

            //the below code is responsible for spawning the coins
            //each enemy defeated has a change to spawn 1-3 coins that are then shot out from their location on death
            //this ensures the coins will no longer go out of bounds
            int CoinCount = Random.Range(1, 3);
            for (int i = 0; i < CoinCount; i++) {
                Vector3 spawnPosition = transform.position;
                float ranX = Random.Range(-.5f, .5f);
                float ranY = Random.Range(-.5f, .5f);
                Vector2 force = new Vector2(ranX, ranY);
                var instance = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                instance.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

                //this line is not neccicary becasue we are spawning in coins with the player already assinged to the prefabs, will keep here tho
                //coinPrefab.GetComponent<coinCounter>().player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }
        }
        else
        {
            Player temp = this.GetComponent<Player>();
            temp.speed = 0;
            Time.timeScale = 0.0f;
            player.playerImageAnimator.SetTrigger("Death"); // game.GameOver() is called by an animation event in the deathAnimation
            // swordAndSpear.SetActive(false);
            // Debug.Log("Disabling " +player.transform.GetChild(0).gameObject.name);
            player.transform.GetChild(0).gameObject.SetActive(false); //get sword ans spear pivot
            // player.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false; //get sword ans spear pivot
            // player.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false; //get sword ans spear pivot
        }

    }


    public void spawnCoins(int ammount){

        for (int i = 0; i < ammount; i++){
            Vector3 spawnPosition = transform.position;
            float ranX = Random.Range(-1f, 1f);
            float ranY = Random.Range(-1f, 1f);
            Vector2 force = new Vector2(ranX, ranY);
            var instance = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            instance.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }



      
    }

    public float GetHealthPercentage()
    {
        return health/maxHealth;
    }
}
