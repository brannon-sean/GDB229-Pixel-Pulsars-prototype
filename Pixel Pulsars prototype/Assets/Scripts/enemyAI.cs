using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("--------- Components --------")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPosition;
    [SerializeField] Transform headPos;

    [Header("--------- Enemy Stats --------")]
    [SerializeField] int healthPoints;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;
 
    [Header("--------- Gun Stats --------")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject loot;

    [Header("--------- Debug --------")]
    private Vector3 playerDirection;
    private bool playerInRange;
    private bool isShooting;
    private float stoppingDistOrig;
    private float angleToPlayer;
    bool destinationChosen;
    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        gamemanager.instance.updateGameGoal(1);
    }
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if(!playerInRange)
        {
            StartCoroutine(roam());
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);
            destinationChosen = false;

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPosition;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }

    }

    bool canSeePlayer()
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
        agent.SetDestination(gamemanager.instance.player.transform.position);
        healthPoints -= amount;
        StartCoroutine(flashDamage());
        if(healthPoints <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
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
            agent.stoppingDistance = 0;
        }
    }
    public void physics(Vector3 push)
    {
        agent.velocity += push / 2;
    }
}
