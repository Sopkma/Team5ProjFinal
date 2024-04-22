using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    public GameObject enemy;
    public Rigidbody2D player;

    private int totalSpawned = 0;
    public int spawnLimit = 1;
    // time in seconds between each creation of an enemy
    public float spawnTimer = 5;
    private float timeLeft = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            if (totalSpawned >= spawnLimit)
            {
                // enemy cap reached, shut down spawner.
                gameObject.SetActive(false);
            }
            else
            {
                MakeEnemy();
                totalSpawned += 1;
                timeLeft = spawnTimer;
            }
        }
    }

    private void MakeEnemy()
    {
        var newEnemy = Instantiate(enemy);

        try
        {
            newEnemy.GetComponent<MeleeEnemy>().player = player;
        }
        catch
        {
            newEnemy.GetComponent<RangedEnemy>().player = player;
        }

        enemy.transform.position = transform.position;

    }
}
