using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltManager : MonoBehaviour
{
    public string[] data; // effect, cast type, level, damage
    public Vector2 fly;

    private bool collided = false;


    void Start()
    {
        StartCoroutine(DeathTimer());
    }


    void Update()
    {

    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(20);
        if (!collided)
        {
            Destroy(gameObject);
        }
    }
}
