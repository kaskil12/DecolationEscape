using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public float AttackRange;
    public float Range;
    bool AttackAble = true;
    public GameObject feet;
    public NavMeshAgent agent;
    public Transform Target;
    public float ScaleZombie;
    public float speeds;
    public float Damage;
    public float Health = 100f;
    public float StartHealth = 100f;
    public Animator animator;
    public bool Initialized = false;
    // Start is called before the first frame update
    public void Initialize(float scale, float speed, float damage, float health){
        Initialized = true;
        ScaleZombie = scale;
        Target = GameObject.Find("Base").transform;
        transform.localScale = new Vector3(scale, scale, scale);
        speeds = speed;
        agent.speed = speed;
        Damage = damage;
        Health = health;
        StartHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Initialized)return;
        // Raycast from the feet in the forward direction
        if (Physics.Raycast(feet.transform.position, feet.transform.forward, out RaycastHit hit, Range, LayerMask.GetMask("Breakable")))
        {
            if (hit.collider.tag == "Breakable" || hit.collider.tag == "Base")
            {
                if (AttackAble)
                {
                    // animator.SetTrigger("Attack");
                    agent.speed = 0;
                    agent.SetDestination(transform.position);
                    StartCoroutine(Attack());

                    // Process damage to Breakable or Base
                    if (hit.collider.tag == "Breakable")
                    {
                        hit.transform.GetComponent<Breakable>().TakeDamageBreakable(Damage);
                    }
                    else if (hit.collider.tag == "Base")
                    {
                        hit.transform.GetComponent<Base>().TakeDamageBase(Damage);
                    }
                }
            }
        }
        else
        {
            // If no collision, resume normal speed
            agent.speed = speeds;
            if (Target == null)
            {
                Target = GameObject.Find("Base").transform;
            }
            else
            {
                agent.SetDestination(Target.position);
            }
        }    
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feet.transform.position, Range);
        Gizmos.DrawWireSphere(feet.transform.position, AttackRange);
    }
    IEnumerator Attack(){
        AttackAble = false;
        yield return new WaitForSeconds(1f);
        AttackAble = true;
    }
    public void TakeDamageEnemy(float damage){
        Health -= damage;
        if(Health <= 0){
            PlayerMovement player = GameObject.Find("PlayerFolder").GetComponent<PlayerMovement>();
            Debug.Log(Health + " " + ScaleZombie);
            int MoneyToGive = Mathf.RoundToInt(StartHealth + ScaleZombie);
            player.AddMoney(MoneyToGive);
            Destroy(gameObject);
        }
    }
}
