using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Reflection;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    public Camera MyCam;
    public float Sensitivity;
    float camX;

    [Header("Rigidbody Movement")]
    public Rigidbody rb;
    public float speed;
    public bool Looks;
    public bool Sliding;
    public bool SlidAble;

    [Header("Jumping")]
    public GameObject JumpObject;
    public float jumprad;
    public float JumpPower;
    bool IsGrounded;
    public bool Jumpable = true;

    [Header("Sliding")]
    public bool hasAppliedForce;

    [Header("PlayerComponents")]
    public GameObject HandObject;
    public float Health = 100;
    public float MaxHealth = 100;
    public bool Walking;
    public bool Running;
    public float CapsuleHeight;
    public float WalkSpeed;
    public float RunningSpeed;
    public GameObject HipPosition;
    public GameObject AimPosition;
    public bool Aiming;
    public float HandLerpSpeed;
    [Header("Inventory")]
    public GameObject[] Inventory;
    public float InventoryLerpSpeed;
    public int CurrentObject;

    [Header("Interaction")]
    public float RayLength = 10;
    [Header("UI")]
    public TMP_Text InteractionText;
    //Icons displaying items in inventory
    public Image[] InventoryIcons;
    public TMP_Text MessageText;
    public Image HitEffect;
    public Color HitEffectColor;

    [Header("Audio")]
    public AudioSource Footsteps;
    public AudioSource JumpSound;
    public AudioSource LandSound;
    public AudioSource KeyPickup;
    public AudioSource HitSound;
    [Header("Arms")]
    public GameObject LeftTarget;
    public GameObject RightTarget;
    public float ArmLerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayer();
    }

    void InitializePlayer()
    {
        Aiming = false;
        SlidAble = true;
        Jumpable = true;
        Health = 100;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Looks = true;
        speed = 50;
        MessageText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        SlidingMechanics();
        Jumping();
        UpdateHitEffectColor();
        if (Inventory[CurrentObject] != null)
        {
            // Inventory[CurrentObject].transform.position = Vector3.Lerp(Inventory[CurrentObject].transform.position, HandObject.transform.position, InventoryLerpSpeed);
            // Inventory[CurrentObject].transform.rotation = Quaternion.Slerp(Inventory[CurrentObject].transform.rotation, HandObject.transform.rotation, InventoryLerpSpeed);
            //Make gun sway when looking around
            Inventory[CurrentObject].transform.rotation = Quaternion.Slerp(Inventory[CurrentObject].transform.rotation, HandObject.transform.rotation, InventoryLerpSpeed * Time.deltaTime);

        }
        if(Inventory[CurrentObject] != null){
            foreach(Transform transform in Inventory[CurrentObject].transform) {
                if(transform.CompareTag("LeftGrip")) {
                    LeftTarget.transform.position = transform.position;
                    LeftTarget.transform.rotation = transform.rotation;
                    
                }else if(transform.CompareTag("RightGrip")) {
                    RightTarget.transform.position = transform.position;
                    RightTarget.transform.rotation = transform.rotation;
                    break;
                }
            }
        }else{
            LeftTarget.transform.position = Vector3.Lerp(LeftTarget.transform.position, HandObject.transform.position, ArmLerpSpeed * Time.deltaTime);
            RightTarget.transform.position = Vector3.Lerp(RightTarget.transform.position, HandObject.transform.position, ArmLerpSpeed * Time.deltaTime);
        }
        if(Health < MaxHealth){
            Health += 1f * Time.deltaTime;
        }
        if(rb.velocity.magnitude > 0.1f && IsGrounded && !Footsteps.isPlaying && !Running){
            Footsteps.Play();
        }else if(rb.velocity.magnitude < 0.1f && IsGrounded && Footsteps.isPlaying){
            Footsteps.Stop();
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            HandDrop();
        }
        //Change current item in inventory
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            ChangeCurrentObject(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            ChangeCurrentObject(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            ChangeCurrentObject(2);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Aiming = !Aiming;
        }
        //Raycast to pickup items
        if(Physics.Raycast(MyCam.transform.position, MyCam.transform.forward, out RaycastHit hit, RayLength)){
            if(hit.transform.tag == "Pickup"){
                InteractionText.text = "Press E to pickup " + hit.transform.name;
                if(Input.GetKeyDown(KeyCode.E)){
                    HandPickup(hit.transform.gameObject);
                }
            }else{
                InteractionText.text = "";
            }
        }else{
            InteractionText.text = "";
        }
    }
    public void SendMessage(string Message){
        MessageText.text = Message;
        StartCoroutine(MessageDelay());
    }
    IEnumerator MessageDelay(){
        yield return new WaitForSeconds(3);
        MessageText.text = "";
    }
    void ChangeCurrentObject(int NewCurrentObject){
        if(Inventory[NewCurrentObject] == null){
            Inventory[CurrentObject].SetActive(false);
            CurrentObject = NewCurrentObject;
            return;
        }
        CurrentObject = NewCurrentObject;
        Inventory[CurrentObject].transform.position = HandObject.transform.position;
        Inventory[CurrentObject].transform.rotation = HandObject.transform.rotation;
        Inventory[CurrentObject].SetActive(true);
        foreach(GameObject obj in Inventory){
            if(obj != Inventory[CurrentObject] && obj != null){
                obj.SetActive(false);
            }
        }
    }
    void HandPickup(GameObject Object){
        if(Inventory[CurrentObject] == null){
            Object.GetComponent<Rigidbody>().isKinematic = true;
            Object.GetComponent<Collider>().enabled = false;
            Inventory[CurrentObject] = Object;
            Inventory[CurrentObject].transform.position = HandObject.transform.position;
            Inventory[CurrentObject].transform.rotation = HandObject.transform.rotation;
            if (Inventory[CurrentObject] != null){
                MonoBehaviour[] inventoryScripts = Inventory[CurrentObject].GetComponents<MonoBehaviour>();
                foreach (var script in inventoryScripts)
                {
                    MethodInfo equipMethod = script.GetType().GetMethod("Equip");
                    if (equipMethod != null)
                    {
                        equipMethod.Invoke(script, null);
                        break;
                    }
                }
            }
        }
    }
    public void HandDrop(){
        if(Inventory[CurrentObject] != null){
            if (Inventory[CurrentObject] != null){
                MonoBehaviour[] inventoryScripts = Inventory[CurrentObject].GetComponents<MonoBehaviour>();
                foreach (var script in inventoryScripts)
                {
                    MethodInfo equipMethod = script.GetType().GetMethod("Unequip");
                    if (equipMethod != null)
                    {
                        equipMethod.Invoke(script, null);
                        break;
                    }
                }
            }
            Inventory[CurrentObject].GetComponent<Rigidbody>().isKinematic = false;
            Inventory[CurrentObject].GetComponent<Collider>().enabled = true;
            Inventory[CurrentObject].transform.position = HandObject.transform.position;
            Inventory[CurrentObject].transform.rotation = HandObject.transform.rotation;
            Inventory[CurrentObject] = null;
        }
    }
    void UpdateHitEffectColor()
    {
        float healthPercentage = Health / MaxHealth;
        HitEffectColor.a = 1 - healthPercentage;
        HitEffect.color = HitEffectColor;
    }
    public void TakeDamage(float Damage){
        Health -= Damage;
        HitSound.Play();
        
        if(Health <= 0){
            Die();
        }
    }
    void Die(){
        SendMessage("You Died");
        StartCoroutine(Delay());
    }
    IEnumerator Delay(){
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        //Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void FixedUpdate()
    {
        if(IsGrounded && !Sliding){
            HandleInput();
        }
        ApplySlideForce();
        UpdateHandPosition();
        if (Inventory[CurrentObject] != null)
        {
            // Inventory[CurrentObject].transform.position = Vector3.Lerp(Inventory[CurrentObject].transform.position, HandObject.transform.position, InventoryLerpSpeed);
            // Inventory[CurrentObject].transform.rotation = Quaternion.Slerp(Inventory[CurrentObject].transform.rotation, HandObject.transform.rotation, InventoryLerpSpeed);
            //Make gun sway when looking around
            Inventory[CurrentObject].transform.position = Vector3.Lerp(Inventory[CurrentObject].transform.position, HandObject.transform.position, InventoryLerpSpeed * Time.deltaTime);
            Inventory[CurrentObject].transform.rotation = Quaternion.Slerp(Inventory[CurrentObject].transform.rotation, HandObject.transform.rotation, InventoryLerpSpeed * Time.deltaTime);

        }
    }
    void Look(){
        if (Looks)
        {
            float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.timeScale;
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.timeScale;
            transform.Rotate(transform.up * mouseX);

            camX -= mouseY;
            camX = Mathf.Clamp(camX, -70, 70);
            GetComponentInChildren<Camera>().transform.localRotation = Quaternion.Euler(camX, 0, 0);
        }
    }
    void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = (moveX * transform.right + moveZ * transform.forward).normalized;
        Vector3 move = moveDirection * speed * Time.deltaTime;
        rb.AddForce(move, ForceMode.VelocityChange);

        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded)
        {
            Running = true;
            speed = RunningSpeed;
        }
        else if (IsGrounded)
        {
            speed = WalkSpeed;
            Running = false;
        }

    }
        bool Landed = false;
        void Jumping(){
        IsGrounded = false;
        foreach(Collider i in Physics.OverlapSphere(JumpObject.transform.position, jumprad)){
            if(i.transform.tag != "Player"){
                IsGrounded = true;
                if(!Landed){
                    LandSound.Play();
                    Landed = true;
                }
                break;
            }
        }
        if(IsGrounded){
            if(Input.GetKeyDown(KeyCode.Space) && !Sliding && Jumpable == true){
                StartCoroutine(JumpDelay());
                JumpSound.Play();
                rb.AddForce(transform.up * JumpPower, ForceMode.VelocityChange);
            }
        }
        
        if(!Sliding){
            rb.drag = IsGrounded ? 15 : 0.1f;
        }
    }
    IEnumerator JumpDelay(){
        Jumpable = false;
        yield return new WaitForSeconds(0.1f);
        Landed = false;
        Jumpable = true;
    }

    void SlidingMechanics()
    {
        if (Input.GetKey(KeyCode.C) && IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.C) && !Sliding)
            {
                Sliding = true;
            }

            Sliding = true;
            GetComponentInChildren<CapsuleCollider>().material.dynamicFriction = 0;
            GetComponentInChildren<CapsuleCollider>().material.staticFriction = 0f;
            CapsuleHeight = Mathf.Lerp(CapsuleHeight, 1, 0.05f);
            rb.drag = 0.2f;
            rb.mass = 10;
        }
        else
        {
            Sliding = false;
            GetComponentInChildren<CapsuleCollider>().material.dynamicFriction = 0.6f;
            GetComponentInChildren<CapsuleCollider>().material.staticFriction = 0.6f;
            GetComponentInChildren<CapsuleCollider>().height = CapsuleHeight;
            rb.mass = 1;
            CapsuleHeight = Mathf.Lerp(CapsuleHeight, 2, 0.05f);
            hasAppliedForce = false;
        }

        GetComponentInChildren<CapsuleCollider>().height = CapsuleHeight;
    }

    void ApplySlideForce()
    {
        if (Sliding && !hasAppliedForce && SlidAble && rb.velocity.magnitude > 1)
        {
            Debug.Log("Force");
            StartCoroutine(SlideForceDelay());
            rb.AddForce(transform.forward * 20 * Time.deltaTime, ForceMode.VelocityChange);
            hasAppliedForce = true;
        }
    }

    void UpdateHandPosition()
    {
        if (Aiming)
        {
            Vector3 aimPositionWorld = AimPosition.transform.position;
            Vector3 hipPositionWorld = HipPosition.transform.position;
            HandObject.transform.position = Vector3.Lerp(hipPositionWorld, aimPositionWorld, HandLerpSpeed);
        }
        else if (!Aiming)
        {
            Vector3 aimPositionWorld = AimPosition.transform.position;
            Vector3 hipPositionWorld = HipPosition.transform.position;
            HandObject.transform.position = Vector3.Lerp(aimPositionWorld, hipPositionWorld, HandLerpSpeed);
        }
    }

    IEnumerator SlideForceDelay()
    {
        SlidAble = false;
        yield return new WaitForSeconds(0.4f);
        SlidAble = true;
    }
}
