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
    public bool IsItemPlaced = true;
    public bool IsPriceChanged = true;
    void Start()
    {
        PriceText.text = ItemName + ": " +Price.ToString();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && IsItemPlaced == false){
            IsItemPlaced = true;
            IsPriceChanged = false;
        }else{
            if(Input.GetKeyDown(KeyCode.G) && IsItemPlaced == false){
                IsItemPlaced = false;
                IsPriceChanged = true;
            }
        }
        if(!IsPriceChanged){
            Price *= 2;
            PriceText.text = ItemName + ": " +Price.ToString();
            IsPriceChanged = true;
        }
    }
    // Start is called before the first frame update
    public void BuyItem(){
        PriceText.text = ItemName + ": " +Price.ToString();
        PlayerMovement player = GameObject.Find("PlayerFolder").GetComponent<PlayerMovement>();
        if(player.Money >= Price && player.IsPlacingItem == false){
        IsItemPlaced= false;
        player.BuyItemsPlayer(ObjectToBuy, Price, SpawnOffst, rotation);
        }
    }
}
