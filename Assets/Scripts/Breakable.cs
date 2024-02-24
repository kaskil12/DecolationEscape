using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float Health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0){
            Destroy(gameObject);
        }
    }
    public void TakeDamageBreakable(float damage){
        Health -= damage;
    }
}
