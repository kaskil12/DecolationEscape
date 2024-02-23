using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform Target;
    public float scale;
    public float speed;
    public float Damage;
    public float Health = 100f;
    public bool Initialized = false;
    // Start is called before the first frame update
    public void Initialize(float scale, float speed, float damage, float health){
        Initialized = true;
        Target = GameObject.Find("PlayerFolder").transform;
        transform.localScale = new Vector3(scale, scale, scale);
        agent.speed = speed;
        Damage = damage;
        Health = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(Target == null && Initialized)return;
        agent.SetDestination(Target.position);

    }
    public void TakeDamageEnemy(float damage){
        Health -= damage;
        if(Health <= 0){
            Destroy(gameObject);
        }
    }
}
