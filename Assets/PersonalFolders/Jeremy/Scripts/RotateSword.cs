using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSword : MonoBehaviour
{

    private SwordAttack sword;

    // Start is called before the first frame update
    void Start()
    {
        sword = GameObject.Find("PlayerSword").GetComponent<SwordAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        //if player is not currently swinging the sword
        if (!sword.isAttacking)
        {
            // I had to relearn geometry fro this lmao
            var mouseCord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var deltaX = mouseCord.x - transform.position.x;
            var deltaY = mouseCord.y - transform.position.y;
            var rad = Mathf.Atan2(deltaY, deltaX); // In radians
            var temp = rad * (180 / Mathf.PI);
            int deg = (int)temp;

            // clamp rotation at greater than 90 degress and less than -90 degrees
            if (deg > 90 || deg < -90)
            {
                // if sprite facing Right
                if (transform.localScale.y > 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);

                }
                //deg *= -1;
            }
            else
            {

                // if sprite facing Left
                if (transform.localScale.y < 0)
                {
                    transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);

                }
                //deg *= -1;
            }

            // changes the hitbox's angle and then the position so that the tip of the triangle is
            // always in the center of the player.
            transform.eulerAngles = new Vector3(0, 0, deg);
        }
        
    }
}
