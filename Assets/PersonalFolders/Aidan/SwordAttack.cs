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
    private bool isAttacking = false;

    public PlaySwordSwing swordAnim;


    // drag collider 2D from the sword collider into here
    public Collider2D swordCollider;

    private List<HealthManager> hitEnemies = new ();

    private void Start()
    {
        
    }

    // edited this to enable/disable the gameobject as a whole since the box for me was starting off disabled
    public void Attack()
    {
        // not currently doing an attack
        if (!isAttacking)
        {
            gameObject.SetActive(true);
            print("attacking.");
            Invoke("StopAttack", swingSpeed);
            isAttacking = true;


            swordAnim.SwingSword();
            

        }
        
    }

    
    public void StopAttack()
    {
        isAttacking = false;

        gameObject.SetActive(false);
        hitEnemies = new();
    }

    // deals damage to object with Enemy tag
    private void OnTriggerEnter2D(Collider2D other)
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

    private void FixedUpdate()
    {
        //AimHitbox();
        
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

