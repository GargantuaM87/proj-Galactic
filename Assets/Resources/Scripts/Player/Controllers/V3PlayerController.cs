using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    private float horizontal;
    private Rigidbody rb;
    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;
    PlayerControls controls;
    public bool iFrames = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Dash.performed += ctx => DashAbility();
    }
    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        MoveX(speed);

        dashTime -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
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

    private void MoveX(float speed)
    {
        if (dashTime > 0)
        {
            Vector3 dashForce = -speed * dashSpeed * horizontal * transform.right;
            transform.position += dashForce * Time.deltaTime;
        }
        else
        {
            Vector3 force = -speed * horizontal * transform.right;
            transform.position += force * Time.deltaTime;
            iFrames = false;
        }
    }


}
