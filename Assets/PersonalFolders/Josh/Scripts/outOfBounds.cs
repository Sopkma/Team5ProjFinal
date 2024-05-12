using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class outOfBounds : MonoBehaviour
{
   
    private Player player;
    public float damageTime = 0.7f;
    private bool playerInBounds;
    public AudioClip damageSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource> ();
        player = FindAnyObjectByType(typeof(Player)) as Player;
        playerInBounds = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // collision.transform.position = GameObject.FindWithTag("MazeStart").transform.position;


            //if having trouble with momentum of player hitting puzzel wall on teleport uncomment this
            //collision.attachedRigidbody.velocity = Vector2.zero;
            playerInBounds = true;
            StartCoroutine(DamagePlayer());
            print("in bounds");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInBounds = false;
            StopCoroutine(DamagePlayer());
            print("out of bounds");
        }
    }

    IEnumerator DamagePlayer()
    {
        while (true && playerInBounds)
        {
            if (playerInBounds)
            {
                audioSource.PlayOneShot(damageSound);
                player.Damage(1);
            }
            yield return new WaitForSeconds(damageTime);
        }
    }
}
