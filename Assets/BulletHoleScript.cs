using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBulletHole());
    }

    IEnumerator DestroyBulletHole()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
