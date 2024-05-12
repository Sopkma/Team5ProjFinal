using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class buttonInfo : MonoBehaviour
{

    public int ItemID;
    public TextMeshProUGUI PriceTxt;
    public GameObject shopManage;

    
    
    private void Update(){
        PriceTxt.text =shopManage.GetComponent<shopManagerScript>().shopItem[2, ItemID].ToString();
    }

    
}
