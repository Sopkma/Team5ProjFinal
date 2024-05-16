using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    private GameObject projectile;
    public bool isTracking = false;
    public float lifeTimer = 5f;
    private float timeLeft;
    public float projectileSpeed = 5f;
    public int damage;

    private Vector3 player;
    private Vector3 travelVector;

    // Start is called before the first frame update
    void Start()
    {
        projectile = gameObject;
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
        travelVector = new Vector2(player.x - projectile.transform.position.x, player.y - projectile.transform.position.y);
        timeLeft = lifeTimer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeLeft < 0)
        {
            Destroy(projectile);
        }

        // can add tracking projectile functionality later :)
        if (isTracking)
        {

        }
        else
        {
            projectile.transform.position += travelVector.normalized * projectileSpeed * Time.deltaTime;
        }

        timeLeft -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collided with: " + collision.name);

        // do damage to player (for now, just despawn and say in console it hit the player
        if (collision.CompareTag("Player"))
        {
            //print("Player Hit!");
            Destroy(projectile);

            // get access to enemy Health 
            Player enemy = collision.GetComponent<Player>();

            if (enemy != null)
            {
                // player is hit, do damage
                enemy.Damage(damage);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            // do nothing
        }
        else if (collision.CompareTag("Ground")) 
        {
            // do nothing
        }
        else
        {
            // hit somthing that is not an enemy or the ground.
            Destroy(projectile);
        }
        
    }
}
