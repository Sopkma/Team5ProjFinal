using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSword : MonoBehaviour
{

    private SwordAttack sword;

    private GameObject player;

    public GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        // gets active weapon
        sword = GetComponentInChildren<SwordAttack>(false);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (parentObject.CompareTag("Player"))
        {
            // print($"<color=blue>{sword.isAttacking}</color>");
        }
        //if entity is not currently swinging the sword
        if (!sword.isAttacking)
        {

            if (parentObject.CompareTag("Enemy"))
            {
                var deltaX = player.transform.position.x - transform.position.x;
                var deltaY = player.transform.position.y - transform.position.y;
                var rad = Mathf.Atan2(deltaY, deltaX);  // radians
                var temp = rad * (180 / Mathf.PI);      // degrees
                int deg = (int)temp;

                if (sword.name.Contains("Spear"))
                {
                    // don't flip based on angle
                }
                else if (sword.name.Contains("Sword"))
                {
                    // clamp rotation at greater than 90 degress and less than -90 degrees
                    if (deg > 90 || deg < -90)
                    {
                        // if sprite facing Right
                        if (transform.localScale.y > 0)
                        {
                            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
                        }
                    }
                    else
                    {
                        // if sprite facing Left
                        if (transform.localScale.y < 0)
                        {
                            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
                        }
                    }
                }
                
                // changes the hitbox's angle and then the position so that the tip of the triangle is
                // always in the center of the player.
                
                transform.eulerAngles = new Vector3(0, 0, deg);
            }
            // player using the sword
            else
            {
                // I had to relearn geometry for this lmao
                var mouseCord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var deltaX = mouseCord.x - transform.position.x;
                var deltaY = mouseCord.y - transform.position.y;
                var rad = Mathf.Atan2(deltaY, deltaX); // In radians
                var temp = rad * (180 / Mathf.PI);
                int deg = (int)temp;

                if (sword.name.Contains("Spear"))
                {
                    // don't flip based on angle
                }
                else if (sword.name.Contains("Sword"))
                {
                    // clamp rotation at greater than 90 degress and less than -90 degrees
                    if (deg > 90 || deg < -90)
                    {
                        // if sprite facing Right
                        if (transform.localScale.y > 0)
                        {
                            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
                        }
                    }
                    else
                    {
                        // if sprite facing Left
                        if (transform.localScale.y < 0)
                        {
                            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
                        }
                    }
                }

                // changes the hitbox's angle and then the position so that the tip of the triangle is
                // always in the center of the player.
                transform.eulerAngles = new Vector3(0, 0, deg);
            }
        }
    }
    
    public void UpdateWeaponType()
    {
        sword = GetComponentInChildren<SwordAttack>(false);
    }
}
