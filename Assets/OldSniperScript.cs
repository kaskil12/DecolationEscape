using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSniperScript : MonoBehaviour
{
    public bool isEquipped;
    [Header("Misc")]
    public bool isReloading;
    public bool isShooting;
    public Transform ShootPoint;
    public GameObject bulletPref;
    [Header("Sway")]
    public float SwayMultiplier;
    public float SwaySmooth;
    
    [Header("Specifications")]
    public float damage;
    public float ammo;
    public float maxAmmo;
    public float reloadTime;
    public float fireRate;
    public float shootSpeed;
    public float gravityForce;
    public float bulletLifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEquipped)return;
        // float mouseX = Input.GetAxisRaw("Mouse X") * SwayMultiplier;
        // float mouseY = Input.GetAxisRaw("Mouse Y") * SwayMultiplier;
        // Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        // Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        // Quaternion targetRotation = rotationX * rotationY;
        // transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, SwaySmooth * Time.fixedDeltaTime);
        if(Input.GetMouseButtonDown(0))
        {
            if(ammo > 0)
            {
                if(!isShooting)
                {
                    isShooting = true;
                    Shoot();
                    StartCoroutine(ShootDelay());
                }
            }
            else
            {
                Reload();
            }
        }
    }
    public void Equip()
    {
        isEquipped = true;
        Debug.Log("Sniper Equipped");
    }
    public void Unequip()
    {
        isEquipped = false;
        Debug.Log("Sniper Unequipped");
    }
    public void Shoot()
    {
        Debug.Log("Sniper Shot");
        Debug.Log(ShootPoint.position);
        GameObject bullet = Instantiate(bulletPref, ShootPoint.position, ShootPoint.rotation);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        if(bulletScript != null)
        {
            bulletScript.Initialize(ShootPoint, shootSpeed, gravityForce);
        }
        Destroy(bullet, bulletLifeTime);
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    public void Reload()
    {
        Debug.Log("Sniper Reloaded");
    }
}
