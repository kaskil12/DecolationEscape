using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DevScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSpeed();
    }
    void ChangeSpeed(){
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            Time.timeScale += 1;
            Debug.Log(Time.timeScale);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            Time.timeScale -= 1;
            Debug.Log(Time.timeScale);
        }
    }
}
