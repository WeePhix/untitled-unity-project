using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Inputs input;
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed, acceletation, deceleration, maxMomentum, blinkTime, blinkDelay, blinkCD, baseBlinkSpeed, blinkSpeed;
    [SerializeField] private Vector2 momentum = Vector2.zero;


    private float blink, blinkStart;
    [SerializeField] private Vector2 move, blinkVector;
    [SerializeField] private bool isBlinking, canBlink = true;


    private void Awake()
    {
        input = new Inputs();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        move = input.Controls.Movement.ReadValue<Vector2>().normalized;
        blink = input.Controls.Blink.ReadValue<float>();
        
        if (move.x == 0 && momentum.x != 0)
        {
            momentum.x -= deceleration * momentum.x / Mathf.Abs(momentum.x);
        }
        else if (move.x > 0)
        {
            momentum.x += acceletation;
        }
        else if (move.x < 0)
        {
            momentum.x -= acceletation;
        }

        if (move.x != 0 && momentum.x != 0 && move.x/Mathf.Abs(move.x) != momentum.x/Mathf.Abs(momentum.x))
        {
            momentum.x -= deceleration * momentum.x / Mathf.Abs(momentum.x);
        }
        

        if (move.y == 0 && momentum.y != 0)
        {
            momentum.y -= deceleration * momentum.y / Mathf.Abs(momentum.y);
        }
        else if (move.y > 0)
        {
            momentum.y += acceletation;
        }
        else if (move.y < 0)
        {
            momentum.y -= acceletation;
        }

        if (move.y != 0 && momentum.y != 0 && move.y / Mathf.Abs(move.y) != momentum.y / Mathf.Abs(momentum.y))
        {
            momentum.y -= deceleration * momentum.y / Mathf.Abs(momentum.y);
        }


        if (momentum.magnitude > maxMomentum)
        {
            momentum = momentum / momentum.magnitude * maxMomentum;
        }
        if (momentum.magnitude < 0.5 && move.magnitude < 0.5)
        {
            momentum = Vector2.zero;
        }


        if (canBlink && blink != 0 && move != Vector2.zero)
        {
            blinkVector = move;
            canBlink = false;
            StartCoroutine(BlinkCoroutine());
        }

        if (isBlinking)
        {
            blinkSpeed = baseBlinkSpeed * (1 - (Time.time - blinkStart) / blinkTime);

            if (blinkSpeed < 0)
            {
                blinkSpeed = 0;
                isBlinking = false;
                input.Enable();
            }
        } 


        rb.velocity = move * moveSpeed + momentum + blinkVector * blinkSpeed;
    }


    private IEnumerator BlinkCoroutine()
    {
        input.Disable();
        yield return new WaitForSeconds(blinkDelay);
        blinkStart = Time.time;
        isBlinking = true;
        yield return new WaitForSeconds(blinkCD);
        canBlink = true;
    }
}