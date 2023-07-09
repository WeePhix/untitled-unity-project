using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltManager : MonoBehaviour
{
    public string[] data; // effect, cast type, level, damage
    public Vector2 fly;


    void Start()
    {
        
    }


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 7)
        {
            collider.gameObject.GetComponent<EnemyManager>().data = data;
            Destroy(gameObject);
        }
    }
}
