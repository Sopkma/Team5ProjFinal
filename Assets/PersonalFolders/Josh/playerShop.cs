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
    void Start(){
      //makes sure that the menues are set to disabled   
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            playerIn = true;
            print("inshop");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Player")){
            playerIn = false;
            print("left shop");
        } 
    }
    private void openShop(){
        //is called when the E key is pressed insie the shop area
        //enables all the UI for the shop
        //disables movment
        print("shop opened");
        isShopOpen = true;
        player.speed = 0;
    }
    private void closeShop(){
        print("shopclose");
        //disables all the shop UI
        //re enables movement
        isShopOpen=false;
        player.speed = 0.35f;
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
