using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SwordAttack : MonoBehaviour
{
    // adjust damage here
    public float damage = 3;
    public float swingSpeed = .3f;
    [HideInInspector] public bool isAttacking { get; private set; }
    public PlaySwordSwing swordAnim;
    public GameObject parent;
    // drag collider 2D from the sword collider into here
    public Collider2D swordCollider;

    private bool isKnockedBack = false;
    public float knockbackDist = 2f;
    private float currentKnockbackDist;
    private Vector3 knockbackDirection;
    private GameObject knockbackTarget;

    private List<HealthManager> hitEnemies = new ();

    private void Start()
    {
        currentKnockbackDist = 0;
    }

    // enables collider of sword on swing
    public void Attack()
    {
        // not currently doing an attack
        if (!isAttacking)
        {
            //gameObject.SetActive(true);
            gameObject.GetComponent<Collider2D>().enabled = true;
            //print("attacking.");
            //Invoke("StopAttack", swingSpeed);
            isAttacking = true;

            swordAnim.SwingSword(swingSpeed);
            StartCoroutine(ForceStop());
        }
        
    }

    
    public void StopAttack()
    {
        //print("attack stop");
        isAttacking = false;

        //gameObject.SetActive(false);
        gameObject.GetComponent<Collider2D>().enabled = false;
        hitEnemies = new();
    }

    // deals damage to object with Enemy tag
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (parent.CompareTag("Player"))
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                // get access to enemy Health 
                HealthManager enemy = other.GetComponent<HealthManager>();

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

                // handle knockback of only Enemy tags, not Boss
                if (other.CompareTag("Enemy"))
                {
                    isKnockedBack = true;
                    knockbackTarget = other.gameObject;
                    knockbackDirection = (new Vector3(other.transform.position.x, other.transform.position.y) - new Vector3(transform.position.x, transform.position.y)).normalized;
                }
            }
        }

        else if (parent.CompareTag("Enemy") || parent.CompareTag("Boss"))
        {
            if (other.tag == "Player")
            {
                // get access to enemy Health 
                HealthManager enemy = other.GetComponent<HealthManager>();

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
    }

    private void Update()
    {
        if (knockbackTarget.IsUnityNull())
        {

        }
        else if (isKnockedBack && currentKnockbackDist < knockbackDist)
        {
            currentKnockbackDist += 5 * Time.deltaTime;
            knockbackTarget.transform.position += knockbackDirection * Time.deltaTime * 6;

        }
        else
        {
            isKnockedBack = false;
            currentKnockbackDist = 0;
        }
    }

    private void OnEnable()
    {
        // moves the hitbox to face the player on the same frame it is activated
        AimHitbox();
    }

    private void AimHitbox()
    {
        // I had to relearn geometry fro this lmao
        var mouseCord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var deltaX = mouseCord.x - transform.position.x;
        var deltaY = mouseCord.y - transform.position.y;
        var rad = Mathf.Atan2(deltaY, deltaX); // In radians
        var temp = rad * (180 / Mathf.PI);
        int deg = 90 + (int)temp;

        // changes the hitbox's angle and then the position so that the tip of the triangle is
        // always in the center of the player.
        transform.eulerAngles = new Vector3(0, 0, deg);
        var y = Mathf.Cos(deg * Mathf.PI / 180) * -1 * 1.15f;
        var x = Mathf.Sin(deg * Mathf.PI / 180) * 1.15f;
        transform.localPosition = new Vector3(x, y, 0);
    }

    private IEnumerator ForceStop()
    {
        yield return new WaitForSeconds(4);

        if (isAttacking)
        {
            isAttacking = false;
        }
    }

    private void MeleeRebound(Collider2D collision)
    {
        float X = 0;
        float Y = 0;
        if (collision.transform.position.x > transform.position.x)
        {
            X = -0.5f;
        }
        else
        {
            X = 0.5f;
        }
        if (collision.transform.position.y > transform.position.y)
        {
            Y = -0.5f;
        }
        else
        {
            Y = 0.5f;
        }
        collision.transform.position += (new Vector3(X, Y) * 10 * Time.deltaTime);
    }

}

