using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyItemScript : MonoBehaviour
{
    public GameObject ObjectToBuy;
    public int Price;
    public float SpawnOffst = 0.5f;
    public Vector3 rotation;
    public string ItemName;
    public TMP_Text PriceText;
    void Start()
    {
        PriceText.text = ItemName + ": " +Price.ToString();
    }
    // Start is called before the first frame update
    public void BuyItem(){
        PriceText.text = ItemName + ": " +Price.ToString();
        PlayerMovement player = GameObject.Find("PlayerFolder").GetComponent<PlayerMovement>();
        if(player.Money >= Price && player.IsPlacingItem == false){
        Price *= 2;
        player.BuyItemsPlayer(ObjectToBuy, Price, SpawnOffst, rotation);
        }
    }
}
