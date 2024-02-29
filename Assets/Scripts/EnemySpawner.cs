using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float MaxPosition = 10f;
    public float MinPosition = -10f;
    public float MaxOffset = 5f;
    public float MinOffset = -5f;   
    public GameObject EnemyPrefab;
    private float MaxScale;
    private float MinScale;
    private float MaxSpeed;
    private float MinSpeed;
    public float MaxHealth = 100f;
    public float MinHealth = 50f;
    public float MaxDamage  = 10f;
    public float MinDamage = 5f;
    public float SpawnTime;
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
        float xOffset = Random.Range(-MaxOffset, MaxOffset);
        float zOffset = Random.Range(-MaxOffset, MaxOffset);
        Vector3 SpawnPoint = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
        GameObject Enemy = Instantiate(EnemyPrefab, SpawnPoint, Quaternion.identity) as GameObject;
        Enemy.GetComponent<Enemy>().Initialize(Random.Range(MinScale, MaxScale), Random.Range(MinSpeed, MaxSpeed), Random.Range(MinDamage, MaxDamage), Random.Range(MinHealth, MaxHealth));
        yield return new WaitForSeconds(SpawnTime);
        MaxScale += 0.1f;
        //MinScale += 0.01f;
        MaxSpeed += 0.2f;
        //MinSpeed += 0.01f;
        MaxHealth += 1f;
        //MinHealth += 0.5f;
        if(SpawnTime > 5)SpawnTime -= 0.3f; 
        StartCoroutine(SpawnEnemy());
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(MaxPosition - MinPosition, 0, MaxPosition - MinPosition));
    }
}
