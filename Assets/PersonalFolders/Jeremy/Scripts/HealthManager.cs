using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    // enemy health
    public float health = 1;
    public int points = 5;
    private ScoreManager scoreManager;

    // allows for checking if the enemy is defeated once they are hit
    public float Health
    {
        set
        {
            print(value);
            health = value;

            if (health <= 0)
            {
                Defeated();
            }
        }
        get { return health; }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // defeated enemy gameobject is deleted
    public void Defeated()
    {
        scoreManager.addToScore(points);
        Destroy(gameObject);
    }
}
