using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Globalization;

public class EnemyManager : MonoBehaviour
{
    public string[] data = null; // effect, cast type, level, damage
    public float health;

    private int[] effects = {0, 0, 0, 0, 0, 0, 0, 0, 0}; // charm, confuse, curse, drunk, fire, poison, shield, shock, slow
    private string[] effectNames = { "none", "charm", "confuse", "curse", "drunk", "fire", "poison", "shield", "shock" };
    private int index, level;
    private float damage;
    private string effect;

    void Start()
    {
        
    }


    void Update()
    {
        if (health <= 0) { Destroy(gameObject); }

        if (data != new string[] { })
        {
            effect = data[0];
            level = int.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);
            damage = float.Parse(data[3], CultureInfo.InvariantCulture.NumberFormat);

            index = Array.IndexOf(effectNames, effect);

            if (index != 0) { effects[index-1] += 1 + (int)level/3; }
            if (effect != "shield")
            {
                health -= damage;
                damage = 0;
            }
        }
        data = new string[] { };
    }
}
