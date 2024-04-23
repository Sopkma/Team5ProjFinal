using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    // adjust damage here
    public float damage = 3;

    Vector2 rightAttackOffset;

    // drag collider 2D from the sword collider into here
    public Collider2D swordCollider;

    private void Start()
    {
        rightAttackOffset = transform.position;
    }

    // edited this to enable/disable the gameobject as a whole since the box for me was starting off disabled
    public void AttackRight()
    {
        gameObject.SetActive(true);

        // swordCollider.enabled = true;
        //transform.localPosition = rightAttackOffset;
        print("attack right");
        Invoke ("StopAttack", 0.5f);
    }

    // isnt used yet, want to try to make attack hitbox solely where the player is facing
    public void AttackLeft() 
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        print("attack left");
        StopAttack();
    }

    public void StopAttack()
    {
        gameObject.SetActive(false); 
        // swordCollider.enabled = false;
    }

    // deals damage to object with Enemy tag
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            // deal damage to enemy
            HealthManager enemy = other.GetComponent<HealthManager>();

            if (enemy != null)
            {
                enemy.Health -= damage;
            }
        }
    }
}
