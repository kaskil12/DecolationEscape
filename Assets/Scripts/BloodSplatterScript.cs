using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBloodSplatter());
    }

    // Update is called once per frame
    IEnumerator DestroyBloodSplatter()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
