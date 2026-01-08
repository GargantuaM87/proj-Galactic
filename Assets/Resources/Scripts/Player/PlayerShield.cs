using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShield : MonoBehaviour

{
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject explosionPrefab;
    [SerializeField] List<GameObject> shieldBar = new List<GameObject>();
    [SerializeField] PlayerController playerController;
    private int count;
    private int maxCount;

    private PlayerHealth playerHealth;

    private ParticleSystem part;



    #region MONOBEHAVIOUR API
    void Start()
    {
        Enemy.EnemyDeath += Push;

        maxCount = shieldBar.Count;
        count = shieldBar.Count;

        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        playerHealth = FindAnyObjectByType<PlayerHealth>();
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("EnemyLasers") && !playerController.iFrames)
        {
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            Pull();

            if (count <= 0)
            {
                playerHealth.Pull();
            }

            Destroy(other);
            GameObject explosion = Instantiate(explosionPrefab, collisionEvents[0].intersection, Quaternion.identity);

            ParticleSystem p = explosion.GetComponent<ParticleSystem>();
            var pmain = p.main;
        }
    }
    #endregion

    #region STAT API
    public void Push()
    {
        if (count < maxCount)
        {
            shieldBar[count].SetActive(true);
            count++;
        }
        else
            return;
    }

    public void SetMaxShield()
    {
        for (; count < maxCount; count++)
        {
            shieldBar[count].SetActive(true);
        }
    }

    public void Pull()
    {
        if (count > 0)
        {
            shieldBar[count - 1].SetActive(false);
            count--;
        }
        else
            return;
    }

    #endregion
}

