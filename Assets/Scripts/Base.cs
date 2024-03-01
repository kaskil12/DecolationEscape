using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float Health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamageBase(float damage){
        Health -= damage;
        if(Health <= 0){
            PlayerMovement player = GameObject.Find("PlayerFolder").GetComponent<PlayerMovement>();
            player.TakeDamage(100);
            Destroy(gameObject);
        }
    }
}
