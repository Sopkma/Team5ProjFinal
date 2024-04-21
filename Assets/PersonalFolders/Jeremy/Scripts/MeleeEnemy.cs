using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeEnemy : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D player;
    public float enemySpeed = .2f;

    // distances where enemy movement begins or stops
    public float minDist = 2f;
    public float maxDist = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 distance = new Vector2(player.position.x - rb.position.x, player.position.y - rb.position.y);
        float euclideanDistance = Vector3.Distance(rb.position, player.position);

        // if further than max distance, do nothing
        // this can be used for ranged enemies(?)
        if (Mathf.Abs(euclideanDistance) > maxDist)
        {
            // print("<color=green>Out of aggro range.</color>");
        }
        // if within maximum distance
        else if (Mathf.Abs(euclideanDistance) < maxDist)
        {
            // if closer than minimum distance, stop moving
            // possible melee here
            if (Mathf.Abs(euclideanDistance) < minDist)
            {
                //print("<color=green>Too close to player.</color>");
            }
            else
            {
                //print("<color=red>Moving to player.</color>");
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
