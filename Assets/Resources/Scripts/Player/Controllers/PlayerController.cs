using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] float speed;
    private float vertical;
    private Rigidbody rb;
    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;
    PlayerControls controls;
    public bool iFrames = false;

    public void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Dash.performed += ctx => DashAbility();
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        
        dashTime -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        MoveY(speed);
    }

    private void DashAbility()
    {
        if (dashCoolDownTimer < 0)
        {
            dashCoolDownTimer = dashCoolDown;
            dashTime = dashDuration;
            iFrames = true;
        }
    }

     private void MoveY(float speed)
    {
        if (dashTime > 0)
        {
            Vector3 dashForce = -speed * dashSpeed * vertical * transform.forward;
            rb.linearVelocity = dashForce;
        }
        else
        {
            Debug.Log(vertical);
            Vector3 force = -speed * vertical * transform.forward;
            //rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Force);
            rb.linearVelocity = force;
            iFrames = false;
        }
    }
}
