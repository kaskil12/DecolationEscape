using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float AttackRange;
    public float damage;
    public float AttackSpeed;
    public bool AttackAble = true;
    public Transform ShootPoint;
    public float shootSpeed;
    public float gravityForce;
    public GameObject bulletPref;
    public AudioSource ShootSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRange, LayerMask.GetMask("Enemy"));

        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<Enemy>() != null)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = col.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            Vector3 targetPosition = new Vector3(closestEnemy.position.x, transform.position.y, closestEnemy.position.z);
            transform.LookAt(targetPosition);

            if (AttackAble)
            {
                ShootPoint.LookAt(targetPosition);

                GameObject bullet = Instantiate(bulletPref, ShootPoint.position, ShootPoint.rotation);
                BulletScript bulletScript = bullet.GetComponent<BulletScript>();

                if (bulletScript != null)
                {
                    bulletScript.Initialize(ShootPoint, shootSpeed, gravityForce, damage);
                    Destroy(bullet, 3);
                }

                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        AttackAble = false;
        ShootSound.Play();
        yield return new WaitForSeconds(AttackSpeed);
        AttackAble = true;
    }
}
