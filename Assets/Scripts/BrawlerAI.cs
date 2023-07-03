using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BrawlerAI : MonoBehaviour
{
    private Vector2 moveVec = Vector2.zero, targetVec, aVec;
    private float distTarget, circ, aDecay, aStart;
    [SerializeField] private bool isChasing = true, isAttacking = false, canAttack = true, seesTarget = false;

    [SerializeField] private float bSpeed, aSpeed, aDelay, aTime, aRecover, aCooldown, aDist, marginDist;

    public GameObject target, hand, weapon, attack;
    private GameObject aCurrent;

    private Rigidbody2D rb;
    private DirectionManager dirMan;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirMan = GetComponent<DirectionManager>();

        circ = UnityEngine.Random.value;
        if (circ < 0.5) { circ = -1; }
        else { circ = 1; }
    }

    void Update()
    {
        targetVec = target.transform.position - transform.position;
        distTarget = targetVec.magnitude;
        targetVec.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetVec, distTarget, LayerMask.GetMask("Player"));
        Console.WriteLine(hit.collider);
        Debug.DrawRay(transform.position, targetVec * distTarget, Color.magenta, 0);
        if (hit.collider != null && hit.collider.gameObject.tag == "Player") { seesTarget = true; Debug.DrawRay(transform.position, targetVec * distTarget, Color.red, 0); }
        else { seesTarget = false; }

        if (seesTarget) {
            moveVec = dirMan.to8dir(targetVec);
            if (canAttack && Mathf.Abs(distTarget - aDist) < marginDist)
            {
                StartCoroutine(AttackTimer());
                hand.transform.rotation = (Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(targetVec.y, targetVec.x) * Mathf.Rad2Deg)));
            }
        }

        if (isAttacking)
        {
            aDecay = (Time.time - aStart) / aTime * aSpeed;
            rb.AddForce(aVec * (aSpeed - aDecay));
        }
        if (isChasing) {
            if (distTarget - aDist < -marginDist)
            {
                moveVec = -moveVec;
            }

            if (Mathf.Abs(distTarget - aDist) < marginDist)
            {
                moveVec = new Vector2(moveVec.y, -moveVec.x);
            }
            
            rb.velocity = moveVec * bSpeed;
        }
    }


    IEnumerator AttackTimer()
    {
        aVec = moveVec;
        rb.velocity = Vector2.zero;
        canAttack = false;
        isChasing = false;
        yield return new WaitForSeconds(aDelay);
        isAttacking = true;
        aStart = Time.time;
        aCurrent = Instantiate(attack, weapon.transform);
        yield return new WaitForSeconds(aTime);
        isAttacking = false;
        Destroy(aCurrent);
        yield return new WaitForSeconds(aRecover);
        isChasing = true;
        yield return new WaitForSeconds(aCooldown);
        canAttack = true;
    }
}