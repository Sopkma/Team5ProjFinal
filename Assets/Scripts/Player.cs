using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour{
    [Header("Player Stats")]
    public float speed = 8f;
    public int coinCounter = 0;
    private float dashCooldown;
    private float imunityframeLength;
    public bool isImmune;
    private float originalSpeed;
    private Rigidbody2D rb;
    private Vector3 moveDir;
    [HideInInspector]
    public float setSpeed;
    private float maxdashcd = 1f;
    private bool isDashButtonDown;

    [Header("ImmunityFrame")]
    public bool immuityFromDamage;
    public float immunityFromDamageLength;
    public Color flashColor;
    public Color defaultColor;
    public int numberOfFlashes;
    public float flashduration;

    [Header("Weapon")]
    public SwordAttack swordAttack;
    public SpriteRenderer spearSpriteShaft;

    [Header("Coin")]
    public TextMeshProUGUI coinsTxt;
    public TextMeshProUGUI coinsUITxt;


    [Header("Animation")]
    public Image DashCDIconGray;
    public Animator animator;
    public SpriteRenderer playerImageSprite;

    [Header("Audio")]
    public AudioClip stepSound;
    public float timeBetweenSteps = 0.2f;


    //misc private fields
    private HealthManager playerHealthManager;
    private float tempTime;
    private AudioSource audioSource;

    [HideInInspector]
    public Bounds spawningBounds;


    void Awake(){
        coinCounter = PlayerPrefs.GetInt("coins");
        coinsTxt.text = "Coins: " + coinCounter;
        coinsUITxt.text = "" + coinCounter;
        isImmune = false;
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        setSpeed = speed;
        playerHealthManager = GetComponent<HealthManager>();
        tempTime = 0;
        audioSource = GetComponent<AudioSource>();
    }


//this update will track directio of player, and check for space bar
private void Update(){
        //getting player velocity
        animator.SetFloat("MoveX", rb.velocity.x);
        animator.SetFloat("MoveY", rb.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1){
            animator.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }

        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W)) { moveY = +1f; spearSpriteShaft.sortingOrder = 2;}
        if (Input.GetKey(KeyCode.S)) { moveY = -1f; spearSpriteShaft.sortingOrder = 4;}
        if (Input.GetKey(KeyCode.A)) { moveX = -1f;playerImageSprite.flipX = true;} 
        if (Input.GetKey(KeyCode.D)) { moveX = +1f; playerImageSprite.flipX = false;}
        moveDir = new Vector3(moveX, moveY).normalized;
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0.1f){
            isDashButtonDown = true;
        }

        if (moveDir != new Vector3(0, 0, 0) && tempTime <= 0){
            audioSource.PlayOneShot(stepSound);
            tempTime = timeBetweenSteps;
        }
        else if (moveDir != new Vector3(0, 0, 0)){
            tempTime -= Time.deltaTime;
        }
        else{
            tempTime = 0f;
        }
    }

    //this update will calculate moving and if the dash button is clicked it will teleport the player a set distance
    //if we want immunity frames we put it here, probably disable the rb/collider and then count down a certain number of seconds.
    //also need to add a cd for the dash that also dose some sort of count down.
    private void FixedUpdate(){
        DashCDIconGray.fillAmount = dashCooldown/maxdashcd;
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
        if (imunityframeLength <=.01f){ isImmune = false; }
        if (immunityFromDamageLength <= .01f) { immuityFromDamage = false; }
        
        //constantly reducing interal cooldowns;
        imunityframeLength -= Time.deltaTime;
        dashCooldown -= Time.deltaTime;
        immunityFromDamageLength -= Time.deltaTime;
    }


    // performs attack from swordAttack making collision box enabled then disabled
    public void Attack(){
        var direction = transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        swordAttack.Attack(); //print("Swing");
    }

    public void AddToCoins(int amount){
        coinCounter += amount;
        coinsTxt.text = "Coins: " + coinCounter;
        coinsUITxt.text = ""+coinCounter;
    }

    public void SubtractFromCoins(int ammount)
    {
        coinCounter -= ammount;
        coinsTxt.text = "Coins: " + coinCounter;
        coinsUITxt.text = "" + coinCounter;
    }


    public void IncreaseSpeed(float amount){
        setSpeed += amount;
    }

    public void FreezePlayer(){
        speed = 0;
    }

    public void UnfreezePlayer(){
        speed = setSpeed;
    }

    public void Damage(int amount){
        playerHealthManager.Health -= amount;
        if (!immuityFromDamage){
            damageIFrames();
        }

        
    }

    public void damageIFrames(){
        immuityFromDamage = true;
        immunityFromDamageLength = 1f;
        StartCoroutine(Flash());
        
    }
    private IEnumerator Flash(){
        int temp = 0;
        while(temp < numberOfFlashes)
        {
            playerImageSprite.color = flashColor;
            yield return new WaitForSeconds(flashduration);
            playerImageSprite.color = defaultColor;
            yield return new WaitForSeconds(flashduration);
            temp++;
        }
    }

    public void SaveComponents()
    {
        PlayerPrefs.SetInt("coins", coinCounter);
        PlayerPrefs.Save();
    }

    //this snipet of code as well as the field is no longer used
    //im leaving this code in here if anyone wants to know what room location the player is currnelty in.
    private void OnTriggerEnter2D(Collider2D collision){
        //this collision check will update upon entering a new area with the ground tag. and will update the players currnet bounding area
        if ((collision.CompareTag("Ground") || collision.CompareTag("BossGround")) && CompareTag("Player")){
            Bounds spawnarea = collision.bounds;
            spawningBounds = spawnarea;

        }
    }
}


