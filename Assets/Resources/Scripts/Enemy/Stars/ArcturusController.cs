using System.Collections;
using System.Collections.Generic;
using COMMAND;
using Unity.Mathematics;
using UnityEngine;

public class ArcturusController : Enemy
{
    [SerializeField] private List<GameObject> kuiperBelt;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private float respawnCooldown;
    private float respawnTimer;

    Command shootCommand;
    //Going to delegate all these statements to their own methods soon
    void Start()
    {
        CurrentHealth = HealthBar.Count;
        MaxHealth = HealthBar.Count;

        shootCommand = new ShootCommand(fireSpeed, fireRate, projectilePrefab, shootOrigins, nextFire);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = transform;

        phase = Phases.Base;

        respawnTimer = respawnCooldown;
    }
    void Update()
    {
        respawnTimer -= Time.deltaTime;
        if (phase == Phases.Base)
        {
            Attack();
            if(respawnTimer <= 0) 
            {
                Vector3 tempPos = kuiperBelt[0].transform.position;
                GameObject instance = Instantiate(starPrefab, new Vector3(tempPos.x, tempPos.y, tempPos.z), quaternion.identity);
                respawnTimer = respawnCooldown;
            }
        }
        if (phase == Phases.Elite)
        {
            Attack();
        }

    }
    public override void Attack()
    {
        shootOrigins[0].gameObject.transform.LookAt(player);
        shootCommand.Execute();
    }

    public void BinaryStar()
    {

    }



}
