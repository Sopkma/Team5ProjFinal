using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;


public class Player : MonoBehaviour{

    public float speed = 10f;
    public int coinCounter = 0;
    public float dashCooldown;


    public float imunityframeLength;

    public bool isImmune;

    private Rigidbody2D rb;
    private Vector3 moveDir;
    private bool isDashButtonDown;
    private float originalSpeed;
    private float setSpeed;

    [Header("drag the SwordHitbox object with the SwordAttack script")]
    public SwordAttack swordAttack;

    public TextMeshProUGUI coinsTxt;
    private HealthManager playerHealthManager;


    public Bounds spawningBounds;

    void Awake(){
        isImmune = false;
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        setSpeed = speed;
        playerHealthManager = GetComponent<HealthManager>();
    }


    //this update will track directio of player, and check for space bar
    private void Update(){
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W)) { moveY = +1f; }
        if (Input.GetKey(KeyCode.S)) { moveY = -1f; }
        if (Input.GetKey(KeyCode.A)) { moveX = -1f; }
        if (Input.GetKey(KeyCode.D)) { moveX = +1f; }
        moveDir = new Vector3(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Space)){
            isDashButtonDown = true;
        }
    }

    //this update will calculate moving and if the dash button is clicked it will teleport the player a set distance
    //if we want immunity frames we put it here, probably disable the rb/collider and then count down a certain number of seconds.
    //also need to add a cd for the dash that also dose some sort of count down.
    private void FixedUpdate(){
        rb.velocity = moveDir * speed;
        if (isDashButtonDown && dashCooldown <= 0.1f){
            float dashAmmount = 2.5f;
            Vector3 dashPosition = transform.position + moveDir * dashAmmount;
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, moveDir, dashAmmount);
            if (raycast.collider != null) { dashPosition = raycast.point; }
            rb.MovePosition(transform.position + moveDir * dashAmmount);
            isDashButtonDown = false;
            isImmune = true;
            imunityframeLength = .1f;
            dashCooldown = 1f;
        }
        //checks if the imunity frame length is over and sets it to false
        if (imunityframeLength <=.01f){isImmune = false;}

        //constantly reducing interal cooldowns;
        imunityframeLength -= Time.deltaTime;
        dashCooldown -= Time.deltaTime;
    }


    // performs attack from swordAttack making collision box enabled then disabled
    public void Attack(){
        var direction = transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        swordAttack.Attack(); //print("Swing");
    }

    public void AddToCoins(int amount)
    {
        coinCounter += amount;
        coinsTxt.text = "Coins: " + coinCounter;
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        setSpeed = speed;
    }

    public void FreezePlayer()
    {
        speed = 0;
    }

    public void UnfreezePlayer()
    {
        speed = setSpeed;
    }

    public void Damage(int amount)
    {
        playerHealthManager.Health -= amount;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        //this collision check will update upon entering a new area with the ground tag. and will update the players currnet bounding area
        if (collision.CompareTag("Ground") && CompareTag("Player")){
            Bounds spawnarea = collision.bounds;
            spawningBounds = spawnarea;

        }
    }
}


