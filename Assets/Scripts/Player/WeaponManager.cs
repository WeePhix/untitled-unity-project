using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


class Weapon
{
    string name;            // name of the weapon
    int damage;             // how much HP the projectile removes on impact
    float cooldown;         // how long it takes until you fan fire it again
    int count;              // how many projectiles there are
    float speed;            // how fast the projectile moves
    float size;             // how big it is (in comparison to the prefab)
    float lifetime;         // how long it exists for in air
    float spread;           // the range of angles that can affect the projectile
    float angle;            // the main angle of the projectile's path
    bool followPlayer;      // whether the projectiles position is relative to the player's
    float knockback;        // how far back the projectile send an enemy on impact
    GameObject design;      // what the projectile should look like
    Color color;            // what color should be applied to the prefab


    public Weapon(string name, Color color, GameObject design = null, int damage = 1, float cooldown = 1, int count = 1, float speed = 3, float size = 1, float lifetime = 1, float spread = 0, float angle = 0, bool followPlayer = false, float knockback = 1)
    {
        this.name = name;
        this.damage = damage;
        this.cooldown = cooldown;
        this.count = count;
        this.speed = speed;
        this.size = size;
        this.lifetime = lifetime;
        this.spread = spread;
        this.angle = angle;
        this.followPlayer = followPlayer;
        this.knockback = knockback;
        this.design = design;
        this.color = color;
    }
}


public class WeaponManager : MonoBehaviour
{
    private Inputs input;
    private Rigidbody2D rb;
    private ProjectileManager projectileManager;

    [SerializeField] private bool canFire1, canFire2;
    [SerializeField] private float instantiateProjectileDist, index1, index2;
    [SerializeField] private Weapon primary, secondary;
    [SerializeField] private float[] stats1, stats2;

    public GameObject swordSlash;

    private Weapon sword;
    private Weapon[] weapons = {
            new Weapon(name: "empty", color: Color.white),
            new Weapon(name: "sword", damage: 4, cooldown: 1, lifetime: 0.1f, followPlayer: true, knockback: 3, color:Color.white),
        };

    private Vector2 mousePos, toMouse;
    private GameObject emptyGO;

    private void Awake()
    {
        input = new Inputs();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canFire1 = true; canFire2 = true;

        weapons = {
            new Weapon(name: "empty", design: emptyGO),
            new Weapon(name: "sword", damage: 4, cooldown: 1, lifetime: 0.1f, followPlayer: true, knockback: 3, design: swordSlash, color:Color.white),
        };
    }

    void Update()
    {
        primary = weapons[index1];

        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        toMouse = mousePos - new Vector2 (transform.position.x, transform.position.y);

        if (input.Controls.Primary.ReadValue<float>() == 1 && canFire1)
        {
        }
    }

    private IEnumerator FirePrimary(float[] stats)
    {
        canFire1 = false;

        for (int i = 0; i <= stats[2]; i ++)
        {
            Debug.Log("Fired projectile no. " + i);
            Fire(Random.Range(-stats[6]/2, stats[6]/2), stats[3], stats[4], stats[5], stats[8], stats[7], projectiles[(int)stats[9]], stats[10], stats[11]);
        }

        yield return new WaitForSeconds(stats[1]);
        canFire1 = true;
    }


    private void Fire(float offset, float speed, float size, float lifetime, float knockback, float follows, GameObject design, float hue, float angle)
    {
        offset = Mathf.Deg2Rad * (offset + angle);
        Vector3 aim = new Vector2(toMouse.x * Mathf.Cos(offset) - toMouse.y * Mathf.Sin(offset), toMouse.x * Mathf.Sin(offset) + toMouse.y * Mathf.Cos(offset)).normalized;
        GameObject bullet = Instantiate(design, transform.position + aim * instantiateProjectileDist, Quaternion.LookRotation(aim), transform);
        projectileManager = bullet.GetComponent<ProjectileManager>();
        projectileManager.RecieveData(speed, size, lifetime, knockback, hue, follows, aim);
    }
}
