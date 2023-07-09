using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class BrawlerAI : MonoBehaviour
{
    private Vector2 moveVec = Vector2.zero, targetVec, dVec;
    private float distTarget, circ, dDecay, dStart, angle;
    [SerializeField] private bool isChasing = true, isDashing = false, canDashA = true, canDashD = true, canAttack = false, seesTarget = false;

    [SerializeField] private float bSpeed, dSpeed, dDelay, dTime, dRecover, dCooldown, dSlowdown, dDist, marginDist, aDist, aCooldown, aDelay;

    public GameObject target, hand, weapon, dashAttack, stillAttack, death;
    private GameObject aCurrent;

    private Rigidbody2D rb;
    private DirectionManager dirMan;
    private EnemyAI eai;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirMan = GetComponent<DirectionManager>();
        eai = GetComponent<EnemyAI>();

        circ = UnityEngine.Random.value;
        if (circ < 0.5) { circ = -1; }
        else { circ = 1; }
    }

    void Update()
    {
        distTarget = (target.transform.position - transform.position).magnitude;

        targetVec = eai.moveVec(target.transform, transform);
        
        if (targetVec != Vector2.zero)
        {
            seesTarget = true;
            moveVec = targetVec;
        }
        else seesTarget = false;

        if (seesTarget)
        {
            if (canDashA && canDashD && Mathf.Abs(distTarget - dDist) < marginDist) { StartCoroutine(DashTimer()); }

            if (canAttack && distTarget < aDist) { StartCoroutine(AttackTimer()); }
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
        Debug.DrawRay(transform.position, moveVec * distTarget, Color.red, 0);
        Debug.DrawRay(transform.position, targetVec * distTarget, Color.blue, 0);
    }


    IEnumerator DashTimer()
    {
        dVec = moveVec;
        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg));

        rb.velocity = Vector2.zero;
        canDashD = canAttack = isChasing = false;
        yield return new WaitForSeconds(dDelay);
        isDashing = true;
        dStart = Time.time;
        aCurrent = Instantiate(dashAttack, weapon.transform);
        yield return new WaitForSeconds(dTime);
        isDashing = false;
        yield return new WaitForSeconds(dSlowdown);
        Destroy(aCurrent);
        yield return new WaitForSeconds(dRecover);
        isChasing = canAttack = true;
        yield return new WaitForSeconds(dCooldown);
        canDashD = true;
    }

    IEnumerator AttackTimer()
    {
        canDashA = canAttack = isChasing = false;
        hand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg));
        aCurrent = Instantiate(stillAttack, weapon.transform);
        yield return new WaitForSeconds(0.1f);
        Destroy(aCurrent);
        yield return new WaitForSeconds(aDelay);
        canDashA = isChasing = true;
        yield return new WaitForSeconds(aCooldown);
        canAttack = true;
    }
}