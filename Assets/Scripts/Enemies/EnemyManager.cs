using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Globalization;
using System.Runtime.CompilerServices;

public class EnemyManager : MonoBehaviour
{
    public string[] data = {}; // effect, cast type, level, damage
    public float health;

    private int[] effects = {0, 0, 0, 0, 0, 0, 0, 0, 0}; // charm, confuse, curse, drunk, fire, poison, shield, shock, slow
    private string[] effectNames = { "none", "charm", "confuse", "curse", "drunk", "fire", "poison", "shield", "shock" };
    private int index, level;
    private float damage;
    private string effect;
    private bool xPos;
    private bool yPos;
    private Vector2 outVec;


    void Start()
    {
    }


    void Update()
    {
        if (health <= 0f) { Destroy(gameObject); }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Debug.Log(gameObject + " collided with " + collision.gameObject);

            data = collision.gameObject.GetComponent<BoltManager>().data;

            effect = data[0];
            level = int.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);
            damage = float.Parse(data[3], CultureInfo.InvariantCulture.NumberFormat);
            index = Array.IndexOf(effectNames, effect);

            Destroy(collision.gameObject);

            ProcessData();
            data = new string[] {};
        }
    }


    private void ProcessData()
    {
        if (effect != "shield" && damage != 0f)
        {
            health -= damage;
            damage = 0;
        }

        if (index != 0)
        {
            effects[index - 1] += 1 + (int)level / 3;
        }
    }

    public Vector2 moveVec(Transform target, Transform self)
    {
        bool seesTarget;
        Vector2 targetVec = target.position - self.position;
        float distTarget = targetVec.magnitude;
        targetVec.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(self.position, targetVec, distTarget, LayerMask.GetMask("Player"));
        if (hit.collider != null && hit.collider.gameObject.tag == "Player") { seesTarget = true; }
        else { seesTarget = false; }
        if (seesTarget) { return To8dir(targetVec); }
        return Vector2.zero;
    }


    public Vector2 To8dir(Vector2 uVec)
    {
        if (uVec.x >= 0) { xPos = true; }
        else { xPos = false; }
        if (uVec.y >= 0) { yPos = true; }
        else { yPos = false; }

        uVec.x = Mathf.Abs(uVec.x);
        uVec.y = Mathf.Abs(uVec.y);

        float angle = Mathf.Atan2(uVec.y, uVec.x) * Mathf.Rad2Deg;

        if (angle > 67.5f) { outVec = new Vector2(0, 1); }
        else if (angle < 22.5f) { outVec = new Vector2(1, 0); }
        else { outVec = new Vector2(1, 1).normalized; }

        if (!xPos) { outVec.x *= -1; }
        if (!yPos) { outVec.y *= -1; }

        return outVec.normalized;
    }
}
