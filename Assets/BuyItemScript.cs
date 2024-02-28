using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemScript : MonoBehaviour
{
    public GameObject ObjectToBuy;
    public int Price;
    float SpawnOffst = 0.5f;
    // Start is called before the first frame update
    public void BuyItem(){
        PlayerMovement player = GameObject.Find("PlayerFolder").GetComponent<PlayerMovement>();
        player.BuyItemsPlayer(ObjectToBuy, Price, SpawnOffst);
    }
}
