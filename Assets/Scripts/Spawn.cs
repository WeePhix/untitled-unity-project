using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spawn : MonoBehaviour
{
    private GameObject player, brawler;
    private Controls controls;
    private bool canSpawn = true;

    public GameObject Player, Brawler;

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
        player = Instantiate(Player);
    }

    void Update()
    {
        if (controls.PlayerInput.Primary.ReadValue<float>() == 1f && canSpawn)
        {
            StartCoroutine(SpawnBrawler());
        }
    }

    IEnumerator SpawnBrawler()
    {
        canSpawn = false;
        brawler = Instantiate(Brawler, transform);
        brawler.GetComponent<BrawlerAI>().target = player;
        yield return new WaitForSeconds(1);
        canSpawn = true;
    }
}
