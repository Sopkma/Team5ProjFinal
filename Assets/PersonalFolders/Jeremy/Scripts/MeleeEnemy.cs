using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeEnemy : Enemy
{
    private BoxCollider2D rb;
    private Rigidbody2D velocityHandler;

    public Rigidbody2D player;

    public SwordAttack sword;

    public float enemySpeed = .2f;
    public float timeBetweenAttacks = 1f;
    private float timeBeforeNextAttack;

    // distances where enemy movement begins or stops
    public float minDist = 2f;
    public float maxDist = 5f;

    [Header("Animator In Sprite Child")]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<BoxCollider2D>();
        velocityHandler = GetComponent<Rigidbody2D>();
        timeBeforeNextAttack = .1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(state == EnemyState.NORMAL)
        {
            Vector3 distance = new Vector3(player.position.x - rb.transform.position.x, player.position.y - rb.transform.position.y);
            float euclideanDistance = Vector3.Distance(rb.transform.position, player.position);
            FacePlayer(distance, animator.gameObject);

            // if further than max distance, do nothing
            // this can be used for ranged enemies(?)
            if (Mathf.Abs(euclideanDistance) > maxDist)
            {
                // print("<color=green>Out of aggro range.</color>");
                animator.SetBool("IsWalking", false);
            }
            // if within maximum distance
            else if (Mathf.Abs(euclideanDistance) < maxDist)
            {
                // if closer than minimum distance, stop moving and melee
                if (Mathf.Abs(euclideanDistance) < minDist)
                {
                    if (timeBeforeNextAttack <= 0)
                    {
                        // attack script here
                        sword.Attack();
                        timeBeforeNextAttack = timeBetweenAttacks;
                    }

                }
                else
                {

                    //print("<color=red>Moving to player.</color>");
                    rb.transform.position += (distance.normalized * enemySpeed * Time.deltaTime);
                    // play animation
                    animator.SetBool("IsWalking", true);
                }
            }

            // rb.position += (distance.normalized * enemySpeed * Time.deltaTime);
            // enemy still moves, this just prevents rigidbody shenanigans
            velocityHandler.velocity = Vector3.zero;

            if (timeBeforeNextAttack > 0)
            {
                timeBeforeNextAttack -= Time.deltaTime;
            }

            /* handles flipping the sprite
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
            */
        }
    }

    

}
