using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class shopManagerScript : MonoBehaviour{

    public int[,] shopItem = new int[5, 5];
    public Player player;
    public TextMeshProUGUI coinUI;

    void Start(){
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

    public void Buy(){
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (player.coinCounter >= shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID])
            player.coinCounter -= shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID];
            coinUI.text = "Coins:" + player.coinCounter.ToString();
    }
}
