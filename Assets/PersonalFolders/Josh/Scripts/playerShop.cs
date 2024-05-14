using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerShop : MonoBehaviour{
    bool isShopOpen = false;
    bool playerIn = false;
    public Player player;
    public GameObject shopUI;
    private float spe;
    public GameObject textGuide;
    void Start(){
       //spe = player.speed;
      //makes sure that the menues are set to disabled   
      textGuide.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            playerIn = true;
            print("inshop");
            textGuide.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            playerIn = false;
            print("left shop");
            textGuide.SetActive(false);
        }
    }
    private void openShop(){
        //is called when the E key is pressed insie the shop area
        //enables all the UI for the shop
        //disables movment
        print("shop opened");
        isShopOpen = true;
        //player.speed = 0;
        player.FreezePlayer();
    }
    private void closeShop(){
        print("shopclose");
        //disables all the shop UI
        //re enables movement
        isShopOpen=false;
        //player.speed = spe;
        player.UnfreezePlayer();
    }

    //this one function is called from the Game.cs when the e key is pressed and it handles all the funcions in the playershop script
    public void shopManager(){
        if (isShopOpen && playerIn){
            closeShop();
            shopUI.SetActive(false);
        }
        else if(!isShopOpen && playerIn){
            openShop();
            shopUI.SetActive(true);
        }
    }
}
