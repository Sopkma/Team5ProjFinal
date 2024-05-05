using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCounter : MonoBehaviour
{
    public GameObject cointSelf;
    public Player player;

    AudioSource coinSound;



    private void Start()
    {
      
        coinSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            player.coinCounter += 1;

            coinSound.Play();

            Destroy(cointSelf,0.4f);

            
        }
    }
}
