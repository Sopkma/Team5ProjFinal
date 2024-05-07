using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    private Rigidbody2D rb;

    public GameObject projectile;
    public Rigidbody2D player;
    public float enemySpeed = .2f;
    public float damage = 2f;

    // distances where enemy movement begins or stops
    public float minDist = 6f;
    public float maxDist = 15f;

    public float fireRate = 4f;
    // fire timer tracks the time in between shots (based on fireRate)
    private float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fireTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == EnemyState.NORMAL)
        {
            Vector2 distance = new Vector2(player.position.x - rb.position.x, player.position.y - rb.position.y);
            float euclideanDistance = Vector3.Distance(rb.position, player.position);

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
                }
                // in between max and min distance, move to and attack the player
                else
                {
                    Shoot();
                    rb.position += (distance.normalized * enemySpeed * Time.deltaTime);
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
        if (fireTimer > 0)
        {
            // not ready to shoot yet, timer tick down
            fireTimer -= Time.deltaTime;
        }
        else
        {
            var theProjectile = GameObject.Instantiate(projectile);
            theProjectile.GetComponent<EnemyProjectile>().damage = damage;
            theProjectile.transform.position = transform.position;
            fireTimer = fireRate;
        }
        

    }
}
