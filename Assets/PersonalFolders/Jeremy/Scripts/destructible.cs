using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//class to manage all destrucatable objecs
public class destructible : MonoBehaviour{
    AudioSource Potbreak;
    SpriteRenderer sr;
    BoxCollider2D bc;
    public GameObject coinPrefab;

    void Start(){
       Potbreak = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Weapon")){
            Destroyed();
        }
    }



    private void spawnCoins(int min, int max)
    {

        int spawnChance = Random.Range(0,5);

        if (spawnChance > 3)
        {
            int CoinCount = Random.Range(min, max);
            for (int i = 0; i < CoinCount; i++)
            {
                Vector3 spawnPosition = transform.position;
                float ranX = Random.Range(-.5f, .5f);
                float ranY = Random.Range(-.5f, .5f);
                Vector2 force = new Vector2(ranX, ranY);
                var instance = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                instance.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

    private void Destroyed(){
        if (this.CompareTag("Pot")){
            if (!Potbreak.isPlaying)
            {
                Potbreak.Play();
                sr.enabled = false;
                bc.enabled = false;
                spawnCoins(1,3);
                Destroy(this, .7f);
            }
        }
    }
}
