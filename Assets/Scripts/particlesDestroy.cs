using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroy : MonoBehaviour
{
    private float lifetime = 0.4f;
    void Start()
    {
        StartCoroutine(KillAfterDelay());
    }

    IEnumerator KillAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
