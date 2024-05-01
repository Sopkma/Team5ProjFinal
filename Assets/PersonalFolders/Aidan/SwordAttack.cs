using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SwordAttack : MonoBehaviour
{
    // adjust damage here
    public float damage = 3;

    public float swingSpeed = .3f;
    [HideInInspector]
    public bool isAttacking { get; private set; }

    public PlaySwordSwing swordAnim;
    public GameObject parent;


    // drag collider 2D from the sword collider into here
    public Collider2D swordCollider;

    private List<HealthManager> hitEnemies = new ();

    private void Start()
    {
        
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
            if (other.tag == "Enemy")
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

        else if (parent.CompareTag("Enemy"))
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

}

