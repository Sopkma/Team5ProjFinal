using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int health = 10; //each health is 1/2 a heart, 10 health is 5 hearts
    public int maxHealth = 10;
    public int score;
    public GameObject ui; //used to get health bar
    
    // in order to update the healthbar for healing/damage/whatever, use takeDamage and heal so that the healthbar is updated too


    public void takeDamage(int amount) {
        health -= amount;
        ui.GetComponent<HealthbarScript>().updateHealth(health, maxHealth);
        if (health >= 0){
            //TODO die
        }
    }

    public void heal(int amount){
        health += amount;
        ui.GetComponent<HealthbarScript>().updateHealth(health, maxHealth);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ui.GetComponent<HealthbarScript>().updateHealth(health, maxHealth);
    }
}
