using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargerPathLogic : MonoBehaviour
{

    private Rigidbody2D player;
    private Rigidbody2D rb;
    public GameObject chargePath;
    public GameObject rotatePoint;
    public float hitboxLength = 15f;
    public float chargeTime = 2f;
    private float damage, staticDamageTimer;
    private float chargeBuildup;
    private float currentHitboxLength;
    private float timeBetweenAttacks, timeBeforeNextAttack, dashSpeed, dashDist;
    private Vector3 playerPos;
    private Vector3 initialScale;
    private Vector3 initialPos; 
    private Vector2 distance;

    private bool isCharging = false, dashing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<ChargerEnemy>().player;
        timeBetweenAttacks = GetComponent<ChargerEnemy>().timeBetweenAttacks;
        timeBeforeNextAttack = 0;
        currentHitboxLength = .1f;
        chargePath.transform.localScale = initialScale = new Vector3(currentHitboxLength, chargePath.transform.localScale.y, 0);
        chargePath.transform.localPosition = Vector3.zero;
        chargePath.SetActive(false);
        chargeBuildup = hitboxLength / chargeTime;
        dashSpeed = GetComponent<ChargerEnemy>().enemySpeed * 6;
        rb = GetComponent<Rigidbody2D>();
        damage = GetComponent<ChargerEnemy>().damage;
        dashDist = 0;
        staticDamageTimer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dashing)
        {
            // if it hits a wall or other enemy/object reset the dash
            if (collision.gameObject.CompareTag("Wall"))
            {
                dashing = false;
                timeBeforeNextAttack = timeBetweenAttacks;
                dashDist = 0;
                rb.position += (distance.normalized * -1) * dashSpeed * Time.deltaTime;
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                // do nothing, just pass through enemies muhahaha
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthManager>().Health -= damage;
                dashing = false;
                timeBeforeNextAttack = timeBetweenAttacks;
                dashDist = 0;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if player touches the enemy, it will damage the player even if enemy is not charging.
        if (!dashing && staticDamageTimer <= 0)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                staticDamageTimer = timeBetweenAttacks / 2;
                collision.gameObject.GetComponent<HealthManager>().Health -= damage;
            }
        }
        if (staticDamageTimer > 0) staticDamageTimer -= Time.deltaTime;

        if (dashing)
        {
            // if it hits a wall or other enemy/object reset the dash
            if (collision.gameObject.CompareTag("Wall"))
            {
                dashing = false;
                timeBeforeNextAttack = timeBetweenAttacks;
                dashDist = 0;
                rb.position += (distance.normalized * -1) * dashSpeed * Time.deltaTime;
            }
        }
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (dashing)
        {
            // (playerPos.x > transform.position.x + 0.1f || playerPos.x < transform.position.x - 0.1f) && (playerPos.y > transform.position.y + 0.1f || playerPos.y < transform.position.y - 0.1f)
            if (dashDist < hitboxLength + 0.5f)
            {
                rb.position += (distance.normalized * dashSpeed * Time.deltaTime);
                dashDist = Mathf.Abs(Vector3.Distance(transform.position, initialPos));
            }
            else
            {
                dashing = false;
                timeBeforeNextAttack = timeBetweenAttacks;
                dashDist = 0;
            }
        }

        if (isCharging && !dashing)
        {
            if (chargePath.transform.localScale.x >= hitboxLength)
            {
                // do the charge.
                isCharging = false;
                currentHitboxLength = 0.1f;
                chargePath.SetActive(false);
                dashing = true;
            }
            else
            {
                currentHitboxLength = (Time.deltaTime * chargeBuildup) + chargePath.transform.localScale.x;
                chargePath.transform.localScale = new Vector3(currentHitboxLength, chargePath.transform.localScale.y, 0);

                timeBeforeNextAttack = timeBetweenAttacks;
                chargePath.transform.localPosition = new Vector3(currentHitboxLength / 2, chargePath.transform.localPosition.y, 0);
            }
        }

        if (timeBeforeNextAttack > 0 && !isCharging && !dashing)
        {
            chargePath.transform.localScale = initialScale;
            chargePath.transform.localPosition = Vector3.zero;
            timeBeforeNextAttack -= Time.deltaTime;
            chargePath.SetActive(false);
        }

    }

    public void Charge()
    {
        if (timeBeforeNextAttack <= 0 && !dashing)
        {
            //print("in charge function");
            isCharging = true;
            chargePath.SetActive(true);

            // make charge hitbox face the player
            var deltaX = player.transform.position.x - transform.position.x;
            var deltaY = player.transform.position.y - transform.position.y;
            //var tmp = new Vector3(deltaX, deltaY).normalized;
            var rad = Mathf.Atan2(deltaY, deltaX);  // radians
            var temp = rad * (180 / Mathf.PI);      // degrees
            int deg = (int)temp;
            rotatePoint.transform.localEulerAngles = new Vector3(0, 0, deg);

            playerPos = new Vector3(player.transform.position.x, player.transform.position.y);
            initialPos = new Vector3(transform.position.x, transform.position.y);

            distance = new Vector3(playerPos.x - rb.position.x, playerPos.y - rb.position.y);
        }
    }

    public bool IsDashing()
    {
        return dashing;
    }
    public void EditDashing(bool val)
    {
        dashing = val;
    }

    public bool IsCharging()
    {
        return isCharging;
    }

    public void EditCharging(bool val)
    {
        isCharging = val;
    }
}
