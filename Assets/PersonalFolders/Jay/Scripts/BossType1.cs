using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum MinotaurState
{
    WALKING,
    WINDUP,
    ABOUTTOCHARGE,
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

    public GameObject crachParticleEffect;
    private SpriteRenderer spriteRenderer;

    [Header("Everything for the red line")]
    public Animator hitPathAnimator;
    public Transform hitTransform;
    public SpriteRenderer hitSR;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        healthManager = GetComponent<HealthManager>();
        state = MinotaurState.IDLE;
        musicManager = FindAnyObjectByType<MusicManager>();
        enraged = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            // Destroy(gameObject, 1f);
        }
        else if(state == MinotaurState.WINDUP)
        {
            RotateLine();
        }

        if (!enraged && healthManager.GetHealthPercentage() <= 0.5)
        {
            enraged=true;
            chargeUpTime /= 2;
            maxDist *= 2;
            agroDist *= 3;
            hitPathAnimator.speed = 2;
        }
        if(enraged)
        {
            spriteRenderer.color = UnityEngine.Color.red;
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

    // https://forum.unity.com/threads/quaternion-lookrotation-in-2d.292572/
    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void RotateLine()
    {
        Vector2 chargeMiddlePoint = (player.transform.position + this.transform.position)/2;
        hitTransform.position = new Vector3(chargeMiddlePoint.x, chargeMiddlePoint.y, 0);

        float angle = AngleBetweenPoints(transform.position, player.position);
        var targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle+90));
        transform.rotation = Quaternion.Slerp(hitTransform.rotation, targetRotation, 1);

        Vector2 distance = new Vector2(player.position.x - rb.position.x, player.position.y - rb.position.y);
        float euclideanDistance = Vector3.Distance(rb.position, player.position);
        float absEuclideanDistance = Mathf.Abs(euclideanDistance);
        hitTransform.localScale = new Vector3(1, absEuclideanDistance/4, 1);
    }

    private IEnumerator Windup()
    {
        hitPathAnimator.Play("BossLine");
        audioSource.PlayOneShot(growl);
        hitSR.enabled = true;
        yield return new WaitForSeconds(chargeUpTime);
        if (state != MinotaurState.DEFEATED && state != MinotaurState.DAZED)
        {
            state = MinotaurState.ABOUTTOCHARGE;
            chargeDirection = (player.transform.position - this.transform.position).normalized;
            StartCoroutine(StartCharge());
        }
    }

    private IEnumerator StartCharge()
    {
        // audioSource.PlayOneShot(growl);
        hitSR.enabled = true;
        yield return new WaitForSeconds(0.4f);
        if (state != MinotaurState.DEFEATED && state != MinotaurState.DAZED)
        {
            hitSR.enabled = false;
            state = MinotaurState.CHARGING;
            // savedPlayerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            // chargeDirection = (player.transform.position - this.transform.position).normalized;
            StartCoroutine(ChargeStop());
        }
    }

    private IEnumerator ChargeStop()
    {
        yield return new WaitForSeconds(chargeTime);
        hitEnemies = new();
        if (state != MinotaurState.DAZED && state != MinotaurState.DEFEATED)
        {
            print("<color=red>Times Up</color>");
            state = MinotaurState.WALKING;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == MinotaurState.CHARGING && !collision.gameObject.CompareTag("Player"))
        {
            Instantiate(crachParticleEffect, transform.position, Quaternion.identity);
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
        if (state != MinotaurState.DEFEATED)
        {
            state = MinotaurState.WALKING;
        }
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
