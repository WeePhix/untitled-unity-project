using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class BrawlerAI : MonoBehaviour
{
    private Vector2 lastDir = Vector2.zero;
    private Vector2 dirToTarget, attackDir;
    private float distToTarget;
    private Rigidbody2D rb;
    private bool isChasing = true, isAttacking = false, canAttack = true, seesTarget = false;
    private float circleDir;

    [SerializeField] private float baseSpeed, attackSpeed, attackDelay, attackTime, attackRecover, attackCooldown, attackDecay, attackStart, optimalDist, distMargin;

    public GameObject target, hand, weapon, attack, obstacle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleDir = UnityEngine.Random.value;
        if (circleDir < 0.5) { circleDir = -90; }
        else { circleDir = 90; }
    }

    void Update()
    {
        dirToTarget = target.transform.position - this.gameObject.transform.position;
        distToTarget = dirToTarget.magnitude;
        dirToTarget.Normalize();

        RaycastHit2D ray = Physics2D.Raycast(this.gameObject.transform.position, dirToTarget, distToTarget);

        if (ray.collider != null && ray.transform == target.transform)
        {
            lastDir = dirToTarget;
            seesTarget = true;
        }
        else if (ray.collider != null && ray.transform == obstacle.transform) { seesTarget = false; }

        if(isAttacking)
        {
            //attackDecay = attackSpeed / attackTime * (Time.time - attackStart);
            rb.velocity = (( 1 - 1 / attackTime * (Time.time - attackStart)) * attackSpeed * attackDir);
        }

        if (isChasing)
        {
            if (Math.Abs(distToTarget - optimalDist) < distMargin) 
            {
                if (canAttack && seesTarget) { StartCoroutine(AttackTimer()); }
                else
                {
                    lastDir = new Vector2(lastDir.x * Mathf.Cos(circleDir) - lastDir.y * Mathf.Sin(circleDir), lastDir.x * Mathf.Sin(circleDir) + lastDir.y * Mathf.Cos(circleDir));
                }
            }
            else if (distToTarget < optimalDist - distMargin)
            {
                lastDir = -lastDir;
            }

            rb.velocity = lastDir * baseSpeed;
        }
    }

    private IEnumerator AttackTimer()
    {
        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg));

        isChasing = false;
        canAttack = false;

        yield return new WaitForSeconds(attackDelay);
        attackStart = Time.time;
        isAttacking = true;
        GameObject currentAttack = Instantiate(attack, weapon.transform);
        
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        Destroy(currentAttack);
        
        yield return new WaitForSeconds(attackRecover);
        isChasing = true;
        
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
