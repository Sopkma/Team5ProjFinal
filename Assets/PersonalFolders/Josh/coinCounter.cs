using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCounter : MonoBehaviour
{
    public GameObject cointSelf;
    public Player player;
    private SpriteRenderer sr;
    private CircleCollider2D circleCollider;

    AudioSource coinSound;



    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        coinSound = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            
            // player.coinCounter += 1;
            Player tempPlayer = collision.GetComponent<Player>();
            // tempPlayer.coinCounter += 1;
            tempPlayer.AddToCoins(1);
            coinSound.Play();
            circleCollider.enabled = false;
            sr.enabled = false;
            Destroy(cointSelf,0.7f);

            
        }
    }
}
