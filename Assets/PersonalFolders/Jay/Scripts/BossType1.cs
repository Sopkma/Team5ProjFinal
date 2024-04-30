using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossType1 : MonoBehaviour
{
    private Rigidbody2D rb;

    public Rigidbody2D player;

    public float enemySpeed = .2f;

    // distances where enemy movement begins or stops
    public float minDist = 2f;
    public float maxDist = 20f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 distance = new Vector2(player.position.x - rb.position.x, player.position.y - rb.position.y);
        float euclideanDistance = Vector3.Distance(rb.position, player.position);

        
    }
}
