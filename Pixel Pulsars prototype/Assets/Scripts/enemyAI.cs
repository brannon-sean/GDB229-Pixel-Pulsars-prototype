using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPosition;

    [SerializeField] int healthPoints;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject loot;

    private Vector3 playerDirection;
    private bool playerInRange;
    private bool isShooting;

    void Start()
    {
    }
    void Update()
    {
        if (playerInRange)
        {
            playerDirection = gamemanager.instance.player.transform.position - transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
            }

            agent.SetDestination(gamemanager.instance.player.transform.position);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPosition.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        healthPoints -= amount;
        StartCoroutine(flashDamage());
        if(healthPoints <= 0)
        {
            GameObject newItem = Instantiate(loot, transform.position, Quaternion.identity);
            newItem.transform.parent = null;
            Destroy(gameObject);
            
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * targetFaceSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
