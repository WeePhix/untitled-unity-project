using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private Rigidbody2D rb, parentRB;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecieveData(float speed, float size, float lifetime, float knockback, float hue, float follows, Vector2 direction)
    {
        parentRB = transform.parent.GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed + (parentRB.velocity * follows);
        transform.localScale = Vector3.one * size;
        StartCoroutine(Kill(lifetime));

        //compute color
        hue /= 60;
        float X = 1 - Mathf.Abs((hue % 2) - 1);
        float R, G, B;
        if (hue <= 1 || hue >= 5) { R = 1; }
        else if (hue >= 2 && hue <= 4) { R = 0; }
        else { R = X;}

        if (hue >= 1 && hue <= 3) { G = 1; }
        else if (hue >=4) { G = 0; }
        else { G = X;}

        if (hue <= 2) { B = 0; }
        else if (hue >= 3 && hue <= 5) { B = 1;}
        else { B = X;}

        sprite.color = new Color(R, G, B, 1);
    }

    private IEnumerator Kill(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
