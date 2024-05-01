using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinotaurState
{
    WALKING,
    WINDUP,
    CHARGING,
    DAZED
}

public class BossType1 : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D player;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = MinotaurState.WALKING;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == MinotaurState.WALKING)
        {
            Walking();
        }
        if (state == MinotaurState.CHARGING)
        {
            Charging();
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
            // if closer than minimum distance, stop moving
            // possible melee here
            if (absEuclideanDistance < minDist)
            {
                // print("<color=green>Too close to player.</color>");
            }
            else if (absEuclideanDistance < agroDist)
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
            state = MinotaurState.DAZED;
            StartCoroutine(DazeStop());
        }
        if (state == MinotaurState.CHARGING && collision.gameObject.CompareTag("Player"))
        {
            // Damage player
        }
        
    }

    private IEnumerator DazeStop()
    {
        yield return new WaitForSeconds(chargeTime);
        state = MinotaurState.WALKING;
    }

    private void OnDestroy()
    {
        endTrigger.SetActive(true);
    }
}
