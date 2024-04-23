using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    // enemy health
    public float health = 1;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // defeated enemy gameobject is deleted
    public void Defeated()
    {
        Destroy(gameObject);
    }
}
