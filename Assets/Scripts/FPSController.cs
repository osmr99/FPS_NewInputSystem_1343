using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    // references
    CharacterController controller;
    [SerializeField] GameObject cam;
    [SerializeField] Transform gunHold;
    [SerializeField] Gun initialGun;

    // stats
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;

    // private variables
    Vector3 origin;
    Vector3 velocity;
    bool grounded;
    float xRotation;
    Gun currentGun = null;
    bool isSprinting = false;
    bool isFiringHold = false;
    Vector2 movement;
    Vector2 looking;

    // public variables
    public List<Gun> equippedGuns = new List<Gun>();
    public int gunIndex = 0;
    [SerializeField] public int damage = 20;

    // properties
    public GameObject Cam { get { return cam; } }

    // Unity Events
    public UnityAction interaction;
    public UnityEvent<int> takingDamage;
    public UnityEvent flashRed;


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // start with a gun
        if(initialGun != null)
            AddGun(initialGun);

        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Look();
        //HandleSwitchGun(); With the new input system, this method is not needed.
        FireGun();

        // always go back to "no velocity"
        // "velocity" is for movement speed that we gain in addition to our movement (falling, knockback, etc.)
        Vector3 noVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.Lerp(velocity, noVelocity, 5 * Time.deltaTime);
    }

    void Movement()
    {
        grounded = controller.isGrounded;

        if(grounded && velocity.y < 0)
        {
            velocity.y = -1;// -0.5f;
        }

        //movement = GetPlayerMovementVector();
        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        controller.Move(move * movementSpeed * (isSprinting ? 2 : 1) * Time.deltaTime);

        //if (Input.GetButtonDown("Jump") && grounded)
        //{
        //velocity.y += Mathf.Sqrt (jumpForce * -1 * gravity);
        //}

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        //looking = GetPlayerLook();
        float lookX = looking.x * lookSensitivityX * Time.deltaTime;
        float lookY = looking.y * lookSensitivityY * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    //void HandleSwitchGun()
    //{
        //if (equippedGuns.Count == 0)
        //    return;

        //if(Input.GetAxis("Mouse ScrollWheel") > 0) // How
        //{
        //    gunIndex++;
        //    if (gunIndex > equippedGuns.Count - 1)
        //        gunIndex = 0;

        //    EquipGun(equippedGuns[gunIndex]);
        //}

        //else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //{
        //    gunIndex--;
        //    if (gunIndex < 0)
        //        gunIndex = equippedGuns.Count - 1;

        //    EquipGun(equippedGuns[gunIndex]);
        //}
    //}

    /*void FireGun()
    {
        // don't fire if we don't have a gun
        if (currentGun == null)
            return;

        // pressed the fire button
        if(GetPressFire())
        {
            currentGun?.AttemptFire();
        }

        // holding the fire button (for automatic)
        else if(GetHoldFire())
        {
            if (currentGun.AttemptAutomaticFire())
                currentGun?.AttemptFire();
        }

        // pressed the alt fire button
        if (GetPressAltFire())
        {
            currentGun?.AttemptAltFire();
        }
    }*/

    void FireGun()
    {
        // don't fire if we don't have a gun
        if (currentGun == null)
            return;

        if(isFiringHold)
        {
            if (currentGun.AttemptAutomaticFire())
                currentGun?.AttemptFire();
        }

        // pressed the fire button
        //if(isFiring)
        //{
        //    currentGun?.AttemptFire();
            
        //}

        // holding the fire button (for automatic)
        //else if(isFiringHold)
        //{
        //    if (currentGun.AttemptAutomaticFire())
        //        currentGun?.AttemptFire();
        //}

        // pressed the alt fire button
        //if (isAltFiring)
        //{
        //    currentGun?.AttemptAltFire();
        //    isAltFiring = !isAltFiring;
        //}
    }

    public void EquipGun(Gun g)
    {
        // disable current gun, if there is one
        currentGun?.Unequip();
        currentGun?.gameObject.SetActive(false);

        // enable the new gun
        g.gameObject.SetActive(true);
        g.transform.parent = gunHold;
        g.transform.localPosition = Vector3.zero;


        currentGun = g;

        g.Equip(this);
    }

    // public methods

    public void AddGun(Gun g)
    {
        // add new gun to the list
        equippedGuns.Add(g);

        // our index is the last one/new one
        gunIndex = equippedGuns.Count - 1;

        // put gun in the right place
        EquipGun(g);
    }

    public void IncreaseAmmo(int amount)
    {
        currentGun.AddAmmo(amount);
    }

    public void Respawn()
    {
        transform.position = origin;
    }

    // Input methods

    //bool GetPressFire()
    //{
    //  return Input.GetButtonDown("Fire1");
    //}

    //bool GetHoldFire()
    //{
    //return Input.GetButton("Fire1");
    //}

    //bool GetPressAltFire()
    //{
    //return Input.GetButtonDown("Fire2");
    //}

    //bool GetPressFire()
    //{
    //    return OnFire();
    //}

    //bool GetHoldFire()
    //{
    //    return OnFireHold();
    //}

    //bool GetPressAltFire()
    //{
    //    return OnFireAlt();
    //}

    //Vector2 GetPlayerMovementVector() // How
    //{

    //}

    //Vector2 GetPlayerMovementVector() // How
    //{
        //return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    //}

    //Vector2 GetPlayerLook() // How
    //{
        //return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    //}

    //bool GetSprint()
    //{
        //return Input.GetButton("Sprint");
    //}

    //bool GetSprint()
    //{
    //    //return OnSprint();
    //    return true;
    //}

    // Collision methods

    // Character Controller can't use OnCollisionEnter :D thanks Unity
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Damager>())
        {
            var collisionPoint = hit.collider.ClosestPoint(transform.position);
            var knockbackAngle = (transform.position - collisionPoint).normalized;
            velocity = (20 * knockbackAngle);
            takingDamage?.Invoke(damage);
            flashRed?.Invoke();
        }

        if (hit.gameObject.GetComponent <KillZone>())
        {
            Respawn();
        }
    }

    public void OnJump()
    {
        if(grounded)
            velocity.y += Mathf.Sqrt (jumpForce * -1 * gravity);
    }

    public void OnSprint()
    {
        isSprinting = !isSprinting;
    }

    public void OnFire()
    {
        currentGun?.AttemptFire();
    }

    public void OnFireHold()
    {
        isFiringHold = !isFiringHold;
    }

    public void OnFireAlt()
    {
        currentGun?.AttemptAltFire();
    }

    public void OnMovement(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        looking = value.Get<Vector2>();
    }

    public void OnInventoryScroll(InputValue value)
    {
        if (equippedGuns.Count == 0)
            return;
        if (value.Get<float>() > 0)
        {
            gunIndex++;
            if (gunIndex > equippedGuns.Count - 1)
                gunIndex = 0;

            EquipGun(equippedGuns[gunIndex]);
        }
        else if (value.Get<float>() < 0)
        {
            gunIndex--;
            if (gunIndex < 0)
                gunIndex = equippedGuns.Count - 1;

            EquipGun(equippedGuns[gunIndex]);
        }
    }
}