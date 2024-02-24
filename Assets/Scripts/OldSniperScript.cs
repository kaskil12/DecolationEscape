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
    public Camera AimCam;
    public Animator animator;
    [Header("Audio")]
    public AudioSource shootSound;
    public AudioSource reloadSound;

    [Header("Recoil")]
    
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
    [Header("Aiming")]
    public bool isAiming;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEquipped)return;
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if(player.Aiming){
            if(Input.GetKeyDown(KeyCode.V)){
                isAiming = !isAiming;
            }
            if(isAiming){
                AimCam.fieldOfView = Mathf.Lerp(AimCam.fieldOfView, 2, 0.1f);
            }else{
                AimCam.fieldOfView = Mathf.Lerp(AimCam.fieldOfView, 10, 0.1f);
            }
        }else{
            AimCam.fieldOfView = Mathf.Lerp(AimCam.fieldOfView, 40, 0.1f);
        }
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
                    animator.SetTrigger("Shoot");
                    StartCoroutine(ShootDelay());
                }
            }
            else
            {
                Reload();
            }
        }
        if(Input.GetKeyDown(KeyCode.R) && !isReloading && ammo < maxAmmo)
        {
            Reload();
        }
    }
    public void Equip()
    {
        isEquipped = true;
        //.Log("Sniper Equipped");
    }
    public void Unequip()
    {
        isEquipped = false;
        //.Log("Sniper Unequipped");
    }
    public void Shoot()
    {
        ammo--;
        //.Log("Sniper Shot");
        shootSound.Play();
        //.Log(ShootPoint.position);
        GameObject bullet = Instantiate(bulletPref, ShootPoint.position, ShootPoint.rotation);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        if(bulletScript != null)
        {
            bulletScript.Initialize(ShootPoint, shootSpeed, gravityForce, damage);
        }
        Destroy(bullet, bulletLifeTime);
        //Recoil
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        player.Recoil(10);
    }
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    public void Reload()
    {
        //.Log("Sniper Reloaded");
        if(ammo < maxAmmo)
        {
            if(!reloadSound.isPlaying)reloadSound.Play();
            isReloading = true;
            StartCoroutine(ReloadDelay());
        }
    }
    IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        isReloading = false;
    }
}
