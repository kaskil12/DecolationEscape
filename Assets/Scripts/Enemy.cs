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
    public float scale;
    public float speeds;
    public float Damage;
    public float Health = 100f;
    public Animator animator;
    public bool Initialized = false;
    // Start is called before the first frame update
    public void Initialize(float scale, float speed, float damage, float health){
        Initialized = true;
        Target = GameObject.Find("Base").transform;
        transform.localScale = new Vector3(scale, scale, scale);
        speeds = speed;
        agent.speed = speed;
        Damage = damage;
        Health = health;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Initialized)return;
        if(Physics.Raycast(transform.position, Target.position - feet.transform.position, out RaycastHit hit, Range)){
            if(hit.collider.tag == "Breakable"){
                if(AttackAble){
                    // animator.SetTrigger("Attack");
                    agent.speed = 0;
                    StartCoroutine(Attack());
                    hit.transform.GetComponent<Breakable>().TakeDamageBreakable(Damage);
                }
                
            }else if(hit.collider.tag == "Base"){
                if(AttackAble){
                    // animator.SetTrigger("Attack");
                    agent.speed = 0;
                    StartCoroutine(Attack());
                    hit.transform.GetComponent<Base>().TakeDamageBase(Damage);
                }
                
            } else{
                agent.speed = speeds;
            }
        }
        if(Target == null){
            Target = GameObject.Find("Base").transform;
        }else{
            agent.SetDestination(Target.position);
        }

    }
    void DrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
    IEnumerator Attack(){
        AttackAble = false;
        yield return new WaitForSeconds(1f);
        AttackAble = true;
    }
    public void TakeDamageEnemy(float damage){
        Health -= damage;
        if(Health <= 0){
            Destroy(gameObject);
        }
    }
}
