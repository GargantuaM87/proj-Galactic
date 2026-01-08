using System;
using System.Collections;
using System.Collections.Generic;
using COMMAND;
using UnityEngine;
using UnityEngine.UI;

public enum Phases
{
    Introduction,
    Base,
    Elite,
    Superior,
    End
};

public class Enemy : SpaceshipBase
{
    public Phases phase;
    public GameObject projectilePrefab;
    public float points;
    public float targetDistance;
    protected Transform player;
    protected Transform enemy;
    protected float nextFire;
    protected Vector3 distance;
    private float distanceFrom;

    [Header("Fire INFO")]
    [SerializeField] protected float fireSpeed = 100f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected List<GameObject> shootOrigins;

    [Header("UI")]
    [SerializeField] private Image healthbarSprite;

    [Header("VFX")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private String explosionSound;

    [Header("Wave INFO")]
    [SerializeField] private WaveController parentWave; //Referencing wave controller

    Command shootCommand;
    //ACTIONS
    public static event Action EnemyDeath;
    //Reference to ScoreManager
    public static ScoreManager scoreManager;

    private void Start()
    {
        shootCommand = new ShootCommand(fireSpeed, fireRate, projectilePrefab, shootOrigins, nextFire);

        CurrentHealth = HealthBar.Count;
        MaxHealth = HealthBar.Count;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = transform;
        phase = Phases.Base;

        scoreManager = GameObject.FindFirstObjectByType<ScoreManager>();
    }

    private void Update()
    {
        Attack();
    }

    public virtual void Attack()
    {
        //If PLAYER is in RANGE logic 
        distance = (enemy.position - player.position);
        distance.y = 0;
        distanceFrom = distance.magnitude;
        distance /= distanceFrom;

        if (distanceFrom < targetDistance)
        {
            enemy.LookAt(player);
            shootCommand.Execute();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Lasers"))
        {
            Pull();

            if (CurrentHealth <= 0)
            {
                DestroyEnemy();
            }
        }
        else if (other.gameObject.CompareTag("UltLaser"))
        {
            Pull(other.gameObject.GetComponent<ProjectileAddon>().ProjectAddon.Damage);

            if (CurrentHealth <= 0)
            {
                DestroyEnemy();
            }
        }
    }

    public void DestroyEnemy()
    {
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        Destroy(gameObject);
        EnemyDeath.Invoke();
    }
    public void OnDestroy()
    {
        if (parentWave != null)
            parentWave.enemies.Remove(gameObject);

        scoreManager.UpdateScore(points);
        FindFirstObjectByType<AudioManager>().PlaySound("DestroyShip");
    }
}

