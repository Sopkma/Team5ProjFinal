using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outOfBounds : MonoBehaviour
{
   
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = GameObject.FindWithTag("MazeStart").transform.position;
            
            
            //if having trouble with momentum of player hitting puzzel wall on teleport uncomment this
            //collision.attachedRigidbody.velocity = Vector2.zero;
            

            print("outofbounds");
        }
    }
}
