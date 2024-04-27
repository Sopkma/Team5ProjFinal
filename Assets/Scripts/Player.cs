using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public float speed = 0.35f;
    public int coinCounter = 0;
    private Rigidbody2D rb;
    
    // drag the Sword Hitbox object with the SwordAttack script attached here
    public SwordAttack swordAttack;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate(){
 
        var direction = transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

        if (direction.magnitude > 1.0f){
            direction.Normalize();
        }
        rb.MovePosition(transform.position + direction * speed);
    }
    
    public void Attack(){
        var direction = transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        // transform.rotation = Quaternion.LookRotation(direction);
        // performs attack from swordAttack making collision box enabled then disabled
        swordAttack.AttackRight();
        print("Swing");
    }
}
