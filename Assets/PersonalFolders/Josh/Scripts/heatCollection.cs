using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatCollection : MonoBehaviour
{

    GameObject heartSelf;
    HealthManager player;

    private SpriteRenderer sr;
    private CircleCollider2D CC;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        heartSelf = GetComponent<GameObject>();
        CC = GetComponent<CircleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            player = collision.gameObject.GetComponent<HealthManager>();
            if(player.health != player.maxHealth){
                player.health += 1;
                CC.enabled = false;
                sr.enabled = false;
                Destroy(heartSelf, .7f);
            }
        }
    }


}
