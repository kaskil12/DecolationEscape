using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour
{
    public string SceneName;
    public void LoadScene(){
        SceneManager.LoadScene(SceneName);
    }
}
