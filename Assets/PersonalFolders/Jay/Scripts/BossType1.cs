using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum MinotaurState
{
    WALKING,
    WINDUP,
    CHARGING,
    DAZED,
    DEFEATED,
    IDLE
}

public class BossType1 : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D player;

    public float damage = 3;
    private List<HealthManager> hitEnemies = new();

    public float enemySpeed = .2f;
    public float enemyChargeSpeed = 0.9f;

    // distances where enemy movement begins or stops
    public float minDist = 2f;
    public float maxDist = 20f;
    public float agroDist = 15f;
    private MinotaurState state;
    public float chargeUpTime = 3f;
    public float chargeTime = 5f;
    private Vector2 savedPlayerPos;
    private Vector2 chargeDirection;

    public GameObject endTrigger;

    private AudioSource audioSource;
    public AudioClip growl;
    public AudioClip crash;

    public Image healthBar;

    private MusicManager musicManager;

    private bool enraged;
    private HealthManager healthManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<HealthManager>();
        state = MinotaurState.IDLE;
        musicManager = FindAnyObjectByType<MusicManager>();
        enraged = false;
    }

    public void StartBattle()
    {
        if (state == MinotaurState.IDLE)
        {
            state = MinotaurState.WALKING;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == MinotaurState.WALKING)
        {
            Walking();
        }
        else if (state == MinotaurState.CHARGING)
        {
            Charging();
        }
        else if(state == MinotaurState.DEFEATED)
        {
            endTrigger.SetActive(true);
            healthBar.enabled = false;
            musicManager.PlayOutsideBattle();
            Destroy(gameObject, 1f);
        }

        if (!enraged && healthManager.GetHealthPercentage() <= 0.5)
        {
            enraged=true;
            chargeUpTime /= 2;
            maxDist *= 2;
            agroDist *= 2;
        }
    }
    
    private void Walking()
    {
        Vector2 distance = new Vector2(player.position.x - rb.position.x, player.position.y - rb.position.y);
        float euclideanDistance = Vector3.Distance(rb.position, player.position);
        float absEuclideanDistance = Mathf.Abs(euclideanDistance);

        // if further than max distance, do nothing
        // this can be used for ranged enemies(?)
        if (absEuclideanDistance > maxDist)
        {
            // print("<color=green>Out of aggro range.</color>");
        }
        // if within maximum distance
        else if (absEuclideanDistance < maxDist)
        {
            if (absEuclideanDistance < agroDist)
            {
                state = MinotaurState.WINDUP;
                StartCoroutine(Windup());
            }
            else
            {
                // print("<color=red>Moving to player.</color>");
                rb.position += (distance.normalized * enemySpeed);
            }
        }

        // rb.position += (distance.normalized * enemySpeed * Time.deltaTime);
        // enemy still moves, this just prevents rigidbody shenanigans
        rb.velocity = Vector2.zero;

        // if moving left
        if (distance.x < 0)
        {
            // if sprite facing Right
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
        else
        {
            // if sprite facing Left
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
    }

    private void Charging()
    {
        rb.MovePosition(transform.position + new Vector3(chargeDirection.x, chargeDirection.y, 0) * enemyChargeSpeed);
    }

    private IEnumerator Windup()
    {
        audioSource.PlayOneShot(growl);
        yield return new WaitForSeconds(chargeUpTime);
        state = MinotaurState.CHARGING;
        // savedPlayerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        chargeDirection = (player.transform.position - this.transform.position).normalized;
        print(chargeDirection);
        StartCoroutine(ChargeStop());
    }

    private IEnumerator ChargeStop()
    {
        yield return new WaitForSeconds(chargeTime);
        hitEnemies = new();
        if (state != MinotaurState.DAZED)
        {
            print("<color=red>Times Up</color>");
            state = MinotaurState.WALKING;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == MinotaurState.CHARGING && !collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(crash);
            state = MinotaurState.DAZED;
            StartCoroutine(DazeStop());
        }
        if (state == MinotaurState.CHARGING && collision.gameObject.CompareTag("Player"))
        {
            // Damage player
            // get access to enemy Health 
            HealthManager enemy = collision.gameObject.GetComponent<HealthManager>();

            if (enemy != null)
            {

                if (hitEnemies.Contains<HealthManager>(enemy))
                {
                    // enemy has been hit already this attack, skip.
                    //print("enemy already hit this attack!");
                }
                else
                {
                    // enemy is being hit for the first time this attack, do damage and add to list
                    enemy.Health -= damage;
                    hitEnemies.Add(enemy);
                }
            }
        }
        
    }

    private IEnumerator DazeStop()
    {
        yield return new WaitForSeconds(chargeTime);
        state = MinotaurState.WALKING;
    }

    public void SetHealthBar(float amount)
    {
        healthBar.fillAmount = amount;
    }

    public void ChangeState(MinotaurState state)
    {
        this.state = state;
    }
}
