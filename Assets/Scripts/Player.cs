using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;


public class Player : MonoBehaviour
{

    private enum State {
        Normal,
        Rolling,
    }

    public float speed = 0.35f;
    public int coinCounter = 0;
    private Rigidbody2D rb;

    // boolean to tell the player if they can move or not
    private bool canMove = true;
    private Vector3 rollDirection;
    private State state;
    float rollSpd;

    // drag the Sword Hitbox object with the SwordAttack script attached here
    public SwordAttack swordAttack;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        switch (state) {
            case State.Normal:
                var direction = transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        
                // debug
                //print($"moving {direction}");

                if (direction.magnitude > 1.0f){
                    direction.Normalize();
                }
        

                if (canMove == true) {
                    rb.MovePosition(transform.position + direction * speed);
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    rollDirection = direction;
                    rollSpd = 30f;
                    state = State.Rolling;
                }
                break;
            case State.Rolling:
                rb.velocity = rollDirection * rollSpd;
                break;
        }
        
    }
    
    public void Attack(){
        var direction = transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        // transform.rotation = Quaternion.LookRotation(direction);
        // performs attack from swordAttack making collision box enabled then disabled
        swordAttack.Attack();
        print("Swing");
    }

    public void DodgeRoll() {
        switch (state) {
            case State.Rolling:
                float rollSpdDropMultiplier = 50f;
                rollSpd -= rollSpd * rollSpdDropMultiplier * Time.deltaTime;

                float rollSpdMin = 1f;
                if (rollSpd < rollSpdMin) {
                    state = State.Normal;
                }
                break;
        }
    }
}
