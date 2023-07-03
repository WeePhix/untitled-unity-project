using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static DirectionManager;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementInput, dVec;
    private float dStart, dDecay;
    private bool isDashing = false, canDash = true;

    [SerializeField] private float bSpeed, dSpeed, dDelay, dTime, dCooldown;

    private Rigidbody2D rb;
    private Controls controls;
    private SpriteRenderer render;
    private DirectionManager dirMan;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirMan = GetComponent<DirectionManager>();
        render = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        movementInput = controls.PlayerInput.Movement.ReadValue<Vector2>().normalized;

        if (controls.PlayerInput.Dash.ReadValue<float>() == 1.0 && canDash && movementInput != Vector2.zero)
        { 
            StartCoroutine(DashTimer());
            dVec = movementInput;
        }

        if (isDashing) 
        {
            dDecay = (Time.time - dStart) / dTime * dSpeed;
            rb.AddForce(dVec * (dSpeed - dDecay));

        }


        rb.velocity = movementInput * bSpeed;


    }

    private IEnumerator DashTimer()
    {
        controls.Disable();
        canDash = false;
        yield return new WaitForSeconds(dDelay);
        dStart = Time.time;
        isDashing = true;
        yield return new WaitForSeconds(dTime);
        isDashing = false;
        controls.Enable();
        yield return new WaitForSeconds(dCooldown);
        canDash = true;
    }
}