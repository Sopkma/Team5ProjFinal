using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCounter : MonoBehaviour{
    public GameObject cointSelf;
    public Player player;

    private SpriteRenderer sr;
    private CircleCollider2D circleCollider;

    AudioSource coinSound;



    private void Start(){
        sr = GetComponent<SpriteRenderer>();
        coinSound = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            Player tempPlayer = collision.gameObject.GetComponent<Player>();
            tempPlayer.AddToCoins(1);
            coinSound.Play();
            circleCollider.enabled = false;
            sr.enabled = false;
            Destroy(cointSelf, 0.7f);
        }
    }

    
}
