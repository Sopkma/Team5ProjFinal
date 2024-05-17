using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class shopManagerScript : MonoBehaviour{


    //make seperate controle map for the interact so you can disable other controls but interact


    //initializes the shop item array
    public int[,] shopItem = new int[5, 5];
    public Player player;
    public TextMeshProUGUI coinUI;
    public SwordAttack sword;
    public SwordAttack spear;
    public RotateSword rotateSword;

    public UnityEngine.UI.Image spearImage;
    public UnityEngine.UI.Image SpearImage2;

    void Start(){

        //gets the info of the coin counter an sets its initial value
        coinUI.GetComponent<TextMeshProUGUI>();
        coinUI.text = "Coins:"+player.coinCounter.ToString();
        
        //ID's
        shopItem[1, 1] = 1;
        shopItem[1, 2] = 2;
        shopItem[1, 3] = 3;
        shopItem[1, 4] = 4;

        //price
        shopItem[2, 1] = 1;
        shopItem[2, 2] = 2;
        shopItem[2, 3] = 5;
        shopItem[2, 4] = 10;
    }

    
    void Update(){
        coinUI.text = "Coins:" + player.coinCounter.ToString();
    }

    //will ask the even system for the information on the button you just clicked and will find the information attached to it and reduce the coins you have for the ammount it cost
    public void Buy(){
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (player.coinCounter >= shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID]){
    
            //coinUI.text = "Coins:" + player.coinCounter.ToString();


            //buys health
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 1){
                if (player.GetComponent<HealthManager>().maxHealth != player.GetComponent<HealthManager>().health){
                    player.GetComponent<HealthManager>().health += 1;

                    player.SubtractFromCoins(shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID]);
                }
            }

            //buy speed up
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 2){
                player.IncreaseSpeed(player.setSpeed * .10f);

                player.SubtractFromCoins(shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID]);
            }

            //buy attack up
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 3){
                player.GetComponentInChildren<SwordAttack>().damage += 1;

                player.SubtractFromCoins(shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID]);
            }

            //buy spear
            if (shopItem[1, ButtonRef.GetComponent<buttonInfo>().ItemID] == 4){
                var trans = .5f;
                player.SubtractFromCoins(shopItem[2, ButtonRef.GetComponent<buttonInfo>().ItemID]);

                spearImage.GetComponentInChildren<UnityEngine.UI.Button>().gameObject.SetActive(false);
                spearImage.color = Color.gray;
                SpearImage2.color = Color.gray;
                               
                
                

                spear.gameObject.SetActive(true);
                spear.ShowComponents();
                player.swordAttack = spear;
                rotateSword.UpdateWeaponType();
            }
        }
    }
    }
