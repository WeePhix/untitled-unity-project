using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Controls controls;
    private Vector3 dashDirection;
    private bool canDash;

    [SerializeField] private float baseSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDelayBefore;
    [SerializeField] private float dashDelayAfter;
    [SerializeField] private float dashCooldown;

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
    }


    void Update()
    {
        if (controls.PlayerInput.Dash.ReadValue<float>() == 1.0 && canDash) { StartCoroutine(DashTimer()); }

        movementInput = controls.PlayerInput.Movement.ReadValue<Vector2>();
        rb.velocity = movementInput * baseSpeed;
    }

    private IEnumerator DashTimer()
    {
        controls.Disable();
        canDash = false;
        yield return new WaitForSeconds(dashDelayBefore);
        Dash();
        yield return new WaitForSeconds(dashDelayAfter);
        controls.Enable();
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void Dash()
    {
        Vector3 pointerPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        dashDirection = pointerPos - transform.position;
        dashDirection.z = 0;
        dashDirection = dashDirection.normalized;
        rb.AddForce(dashDirection * dashSpeed);
    }
}
