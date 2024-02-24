using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    private float MaxScale;
    private float MinScale;
    private float MaxSpeed;
    private float MinSpeed;
    public float MaxHealth = 100f;
    public float MinHealth = 50f;
    public float MaxDamage  = 10f;
    public float MinDamage = 5f;
    // Start is called before the first frame update
    void Start()
    {
        MaxScale = 1f;
        MinScale = 0.5f;
        MaxSpeed = 1f;
        MinSpeed = 0.2f;
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy(){
        Vector3 SpawnPoint = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        GameObject Enemy = Instantiate(EnemyPrefab, SpawnPoint, Quaternion.identity) as GameObject;
        Enemy.GetComponent<Enemy>().Initialize(Random.Range(MinScale, MaxScale), Random.Range(MinSpeed, MaxSpeed), Random.Range(MinDamage, MaxDamage), Random.Range(MinHealth, MaxHealth));
        yield return new WaitForSeconds(5);
        MaxScale += 0.1f;
        MinScale += 0.01f;
        MaxSpeed += 0.5f;
        MinSpeed += 0.01f;
        MaxHealth += 1f;
        MinHealth += 0.5f;
        StartCoroutine(SpawnEnemy());
    }
}
