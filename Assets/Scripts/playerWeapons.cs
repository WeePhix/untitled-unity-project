using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    public string pEffect = "none", pCastType = "none", sEffect = "none", sCastType = "none";
    public float boltSpeed, pCooldown, sCooldown, pEffectCooldown, sEffectCooldown, baseDmg;
    public int pLevel = 1, sLevel = 1;
    public GameObject Bolt;

    private bool pCan = true, sCan = true;
    private float castTypeCD, damage;
    private Vector3 mousePos;
    private Vector2 mouseVec, boltVec;
    private GameObject thisBolt;


    private Controls controls;

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
        
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mouseVec = (mousePos - transform.position).normalized;


        if (controls.PlayerInput.Primary.ReadValue<float>() == 1.0 && pCan) { StartCoroutine(pCast()); }
        if (controls.PlayerInput.Secondary.ReadValue<float>() == 1.0 && sCan) { StartCoroutine(sCast()); }
    }

    IEnumerator pCast()
    {
        pCan = false;

        float returnedCD = Cast(pEffect, pCastType, pLevel, pCooldown);

        yield return new WaitForSeconds(pCooldown + returnedCD);
        pCan = true;
    }

    IEnumerator sCast()
    {
        sCan = false;

        float returnedCD = Cast(sEffect, sCastType, sLevel, sCooldown);

        yield return new WaitForSeconds(sCooldown + returnedCD);
        sCan = true;
    }

    private float Cast(string eff, string type, int level, float cd)
    {
        castTypeCD = 0;
        if (type == "none")
        {
            float angle = Random.Range(-1, 1) * 3 / Mathf.Sqrt(level) * Mathf.Deg2Rad;
            boltVec = new Vector2(Mathf.Cos(angle) * mouseVec.x + Mathf.Sin(angle) * mouseVec.y * (-1f), Mathf.Cos(angle) * mouseVec.y + Mathf.Sin(angle) * mouseVec.x).normalized;

            thisBolt = Instantiate(Bolt, gameObject.transform);
            thisBolt.GetComponent<Rigidbody2D>().velocity = boltVec * boltSpeed;
            thisBolt.GetComponent<BoltManager>().data = new string[] { eff, type, level.ToString(), (baseDmg + (level - 1) / 3).ToString() };

            castTypeCD = 0.0f;
        }

        return castTypeCD;
    }
}
