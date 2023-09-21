using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("--- Components ---")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Animator animator;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject loot;

    [Header("--- Enemy Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] float shootRate;
    [SerializeField] int experienceMin;
    [SerializeField] int experienceMax;

    //Animations
    [SerializeField] float animChangeSpeed;

    //Variable Definitions:
    private Vector3 playerDirection;
    private bool playerInRange = true;
    private bool isShooting;
    private float stoppingDistOrig;
    private float angleToPlayer;
    private bool isDead;

    void Start()
    {
        stoppingDistOrig = agent.stoppingDistance;
        gamemanager.instance.updateGameGoal(1);
    }
    void Update()
    {
        float enemyVel = agent.velocity.normalized.magnitude;
        animator.SetFloat("Speed", Mathf.Lerp(animator.GetFloat("Speed"), enemyVel, Time.deltaTime * animChangeSpeed));

        if (playerInRange && canSeePlayer())
        {
           
        }
    }

    bool canSeePlayer()
    {
        if (!isDead)
        {
            playerDirection = gamemanager.instance.player.transform.position - (transform.position - Vector3.down);
            angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(headPos.position, playerDirection, out hit))
            {
                if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
                {
                    agent.stoppingDistance = stoppingDistOrig;
                    agent.SetDestination(gamemanager.instance.player.transform.position);

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        faceTarget();

                        if (!isShooting && angleToPlayer <= shootAngle)
                        {
                            StartCoroutine(shoot());
                        }

                    }
                    return true;
                }
            }
        }
            agent.stoppingDistance = 0;
            return false;
        
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

        if(healthPoints <= 0)
        {
            agent.enabled = false;
            animator.SetBool("Dead", true);
            isDead = true;
            gamemanager.instance.updateGameGoal(-1);
            gamemanager.instance.addExperience(UnityEngine.Random.Range(experienceMin, experienceMax));

            StartCoroutine(DeathCleanup());
        }
        else
        {
            StartCoroutine(flashDamage());
            agent.SetDestination(gamemanager.instance.player.transform.position);
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
    public void physics(Vector3 push)
    {
        agent.velocity += push / 2;
    }

    IEnumerator DeathCleanup()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
