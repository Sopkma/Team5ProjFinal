using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public UnityEngine.UI.Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;


    public void updateHealth(int currentHealth, int funcMaxhealth){
        for (int i = 0; i < hearts.Length; i++) //goes through each heart NOT each hp, i = heart sprite
        {
            if ((i+1)*2 <= currentHealth)
            {
                hearts[i].sprite = fullHeart;
            } else if ((((i+1)*2)-1) == currentHealth)
            {
                hearts[i].sprite = halfHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }
            if ((i*2) < funcMaxhealth)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
