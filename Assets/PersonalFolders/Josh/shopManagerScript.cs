using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class shopManagerScript : MonoBehaviour{


    //make seperate controle map for the interact so you can disable other controls but interact


    //initializes the shop item array
    public int[,] shopItem = new int[5, 5];
    public Player player;
    public TextMeshProUGUI coinUI;

    void Start(){
        //gets the info of the coin counter an sets its initial value
        coinUI.GetComponent<TextMeshProUGUI>();
        coinUI.text = "Coins:"+player.coinCounter.ToString();
        
        //ID's
        shopItem[1, 1] = 1;
        shopItem[1, 2] = 2;
        shopItem[1, 3] = 3;
        
        //price
        shopItem[2, 1] = 1;
        shopItem[2, 2] = 2;
        shopItem[2, 3] = 5;
    }

    
    void Update(){
        coinUI.text = "Coins:" + player.coinCounter.ToString();
    }

    //will ask the even system for the information on the button you just clicked and will find the information attached to it and reduce the coins you have for the ammount it cost
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (player.coinCounter >= shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID])
        {
            player.coinCounter -= shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID];
            coinUI.text = "Coins:" + player.coinCounter.ToString();


            //buys health
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 1)
            {
                print("added health");
                player.GetComponent<HealthManager>().health += 1;
            }

            //buy speed up
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 2)
            {
                player.speed += (player.speed * .10f);

                print("speed increase");
            }

            //buy attack up
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 3)
            {
                player.GetComponentInChildren<SwordAttack>().damage += 1;
                print("incrase attack");
            }
        }
    }
    }
