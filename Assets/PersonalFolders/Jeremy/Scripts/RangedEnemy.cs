using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RangedEnemy : Enemy
{
    private Rigidbody2D rb;

    public GameObject projectile;
    public Rigidbody2D player;
    public Light2D staffGlow;
    public float enemySpeed = .2f;
    public int damage = 1;

    // distances where enemy movement begins or stops
    public float minDist = 6f;
    public float maxDist = 15f;

    public float fireRate = 4f;
    // fire timer tracks the time in between shots (based on fireRate)
    private float fireTimer;

    private bool firstShot = true;

    [Header("Animator In Sprite Child")]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 0;
        firstShot = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == EnemyState.NORMAL)
        {
            Vector2 distance = new Vector2(player.position.x - rb.position.x, player.position.y - rb.position.y);
            float euclideanDistance = Vector3.Distance(rb.position, player.position);
            FacePlayer(distance);

            // if further than max distance, do nothing
            if (Mathf.Abs(euclideanDistance) > maxDist)
            {
                // print("<color=green>Out of aggro range.</color>");

            }
            // if within maximum distance
            else if (Mathf.Abs(euclideanDistance) < maxDist)
            {
                // if closer than minimum distance, stand still and attack player
                if (Mathf.Abs(euclideanDistance) < minDist)
                {
                    Shoot();
                    animator.speed = 0.3f;
                }
                // in between max and min distance, move to and attack the player
                else
                {
                    Shoot();
                    rb.position += (distance.normalized * enemySpeed * Time.deltaTime);
                    animator.speed = 1;
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

    }

    private void Shoot()
    {
        // gives fireTimer a random delay when trying to shoot for the first time.
        // makes it so the player is not instantly hit when a ranged enemy spawns in.
        if (firstShot)
        {
            fireTimer = Random.Range(0.75f,1.75f);
            firstShot = false;
        }

        if (fireTimer > 0)
        {
            staffGlow.pointLightOuterRadius = 1;
            staffGlow.intensity += (fireTimer / fireRate) / 5;
            // not ready to shoot yet, timer tick down
            fireTimer -= Time.deltaTime;
        }
        else
        {
            var theProjectile = GameObject.Instantiate(projectile);
            theProjectile.GetComponent<EnemyProjectile>().damage = damage;
            theProjectile.transform.position = transform.position;
            fireTimer = fireRate;
            staffGlow.intensity = 0;
            staffGlow.pointLightOuterRadius = 2;
        }
        

    }
}
