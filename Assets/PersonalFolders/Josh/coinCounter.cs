using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCounter : MonoBehaviour
{
    public GameObject cointSelf;
    public Player player;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("collision detected");
            player.coinCounter += 1;
            Destroy(cointSelf);


        }
    }
}
