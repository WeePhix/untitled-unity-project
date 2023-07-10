using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class BrawlerAI : MonoBehaviour
{
    private Vector2 moveVec = Vector2.zero, targetVec, dVec;
    private float distTarget, circ, dDecay, dStart;
    [SerializeField] private bool isChasing = true, isDashing = false, canDashA = true, canDashD = true, canAttackA = true, canAttackD = true, seesTarget = false;

    [SerializeField] private float bSpeed, dSpeed, dDelay, dTime, dRecover, dCooldown, dSlowdown, dDist, marginDist, aDist, aCooldown, aDelay;

    public GameObject target, hand, weapon, dashAttack, stillAttack, death;
    private GameObject aCurrent;

    private Rigidbody2D rb;
    private EnemyManager eMan;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        eMan = GetComponent<EnemyManager>();

        circ = UnityEngine.Random.value;
        if (circ < 0.5) { circ = -1; }
        else { circ = 1; }
    }

    void Update()
    {
        distTarget = (target.transform.position - transform.position).magnitude;

        targetVec = eMan.moveVec(target.transform, transform);
        
        if (targetVec != Vector2.zero)
        {
            seesTarget = true;
            moveVec = targetVec;
        }
        else seesTarget = false;

        if (seesTarget)
        {
            if (canDashA && canDashD && Mathf.Abs(distTarget - dDist) < marginDist) { StartCoroutine(DashTimer()); }

            if (canAttackA && canAttackD && distTarget < aDist) { StartCoroutine(AttackTimer()); }
        }

        if (isDashing)
        {
            dDecay = (Time.time - dStart) / dTime * dSpeed;
            rb.velocity = (dVec * (dSpeed - dDecay));
        }
        if (isChasing) {
            for (var i = weapon.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(weapon.transform.GetChild(i).gameObject);
            }

            if (distTarget - dDist < -marginDist)
            {
                moveVec = -moveVec;
            }

            if (Mathf.Abs(distTarget - dDist) < marginDist)
            {
                moveVec = new Vector2(moveVec.y, -moveVec.x)*circ;
            }
            
            rb.velocity = moveVec * bSpeed;
        }
    }


    IEnumerator DashTimer()
    {
        dVec = moveVec;
        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg));

        rb.velocity = Vector2.zero;
        canDashD = canAttackD = isChasing = false;
        yield return new WaitForSeconds(dDelay);
        isDashing = true;
        dStart = Time.time;
        aCurrent = Instantiate(dashAttack, weapon.transform);
        yield return new WaitForSeconds(dTime);
        isDashing = false;
        yield return new WaitForSeconds(dSlowdown);
        Destroy(aCurrent);
        yield return new WaitForSeconds(dRecover);
        isChasing = canAttackD = true;
        yield return new WaitForSeconds(dCooldown);
        canDashD = true;
    }

    IEnumerator AttackTimer()
    {
        canDashA = canAttackA = isChasing = false;
        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg));
        aCurrent = Instantiate(stillAttack, weapon.transform);
        yield return new WaitForSeconds(0.1f);
        Destroy(aCurrent);
        yield return new WaitForSeconds(aDelay);
        canDashA = isChasing = true;
        yield return new WaitForSeconds(aCooldown);
        canAttackA = true;
    }
}