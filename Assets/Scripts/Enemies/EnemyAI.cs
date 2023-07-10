using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;

public class EnemyAI : MonoBehaviour
{
    private DirectionManager dirMan;
    private bool seesTarget;

    void Start()
    {
        dirMan = GetComponent<DirectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector2 moveVec(Transform target, Transform self)
    {
        Vector2 targetVec = target.position - self.position;
        float distTarget = targetVec.magnitude;
        targetVec.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(self.position, targetVec, distTarget, LayerMask.GetMask("Player"));
        if (hit.collider != null && hit.collider.gameObject.tag == "Player") { seesTarget =  true; }
        else { seesTarget = false; }
        if (seesTarget) { return dirMan.to8dir(targetVec); }
        return Vector2.zero;
    }
}
