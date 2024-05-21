using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : Enemy
{

    private Rigidbody2D rb;

    public Rigidbody2D player;

    public float enemySpeed = .2f;
    public float timeBetweenAttacks = 1f;
    private float timeBeforeNextAttack;
    public float damage = 5f;

    // distances where enemy movement begins or stops
    public float minDist = 2f;
    public float maxDist = 5f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeBeforeNextAttack = 0f;
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

            }
            // if within maximum distance
            else if (Mathf.Abs(euclideanDistance) < maxDist)
            {
                // if closer than minimum distance, stop moving and melee
                if (Mathf.Abs(euclideanDistance) < minDist)
                {
                    animator.SetBool("IsWalking", false);
                    if (timeBeforeNextAttack <= 0)
                    {
                        // attack script here
                        GetComponent<ChargerPathLogic>().Charge();
                        timeBeforeNextAttack = timeBetweenAttacks;
                    }
                }
                else
                {
                    // if enemy is dashing or charging, don't move in this script
                    if (GetComponent<ChargerPathLogic>().IsDashing() || GetComponent<ChargerPathLogic>().IsCharging())
                    {
                        animator.SetBool("IsWalking", false);
                    }
                    else
                    {
                        animator.SetBool("IsWalking", true);
                        rb.position += (distance.normalized * enemySpeed * Time.deltaTime);
                    }
                }
            }

            // rb.position += (distance.normalized * enemySpeed * Time.deltaTime);
            // enemy still moves, this just prevents rigidbody shenanigans
            rb.velocity = Vector2.zero;

            if (timeBeforeNextAttack > 0)
            {
                timeBeforeNextAttack -= Time.deltaTime;
            }
        }

    }

}
