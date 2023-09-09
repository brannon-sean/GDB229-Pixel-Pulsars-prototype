using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPhysics
{
    [SerializeField] CharacterController controller;

    [SerializeField] int healthPoints;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpsMax;
    [SerializeField] float gravityValue;
    [SerializeField] int pushBackResolve;

    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] GameObject bulletSpawn;
    [SerializeField] GameObject bulletFlash;

    [SerializeField] float leanSpeed;
    [SerializeField] float leanMaxAngle;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpedTimes;
    private bool isShooting;
    private float currentLeanAngle = 0f;
    private int startHealth;
    private Vector3 pushBack;
    private Vector3 lift;

    private void Start()
    {
        startHealth = healthPoints;
        gamemanager.instance.playerScript.spawnPlayer();
    }

    void Update()
    {
        movement();
        //lean();

        if (Input.GetButtonDown("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }


    }

    void lean()
    {
        if ( Input.GetKey(KeyCode.Q))
        {
            currentLeanAngle = Mathf.MoveTowardsAngle(currentLeanAngle, leanMaxAngle, leanSpeed * Time.deltaTime); 
        } else if (Input.GetKey(KeyCode.E))
        {
            currentLeanAngle = Mathf.MoveTowardsAngle(currentLeanAngle, leanMaxAngle, leanSpeed * Time.deltaTime);
        }
        else
        {
            currentLeanAngle = Mathf.MoveTowardsAngle(currentLeanAngle, 0, leanSpeed * Time.deltaTime);
        }
        transform.localRotation = Quaternion.AngleAxis(currentLeanAngle, Vector3.forward);
    }
    void movement()
    {
        if (pushBack.magnitude > 0.01f)
        {
            pushBack.x = Mathf.Lerp(pushBack.x, 0, Time.deltaTime * pushBackResolve);
            pushBack.y = Mathf.Lerp(pushBack.y, 0, Time.deltaTime * pushBackResolve * 3);
            pushBack.z = Mathf.Lerp(pushBack.z, 0, Time.deltaTime * pushBackResolve);
        }
        if (lift.magnitude > 0.01f) 
        {
            lift.x = Mathf.Lerp(lift.x, 0, Time.deltaTime * pushBackResolve * 999);
            lift.y = Mathf.Lerp(lift.y, 0, Time.deltaTime * pushBackResolve);
            lift.z = Mathf.Lerp(lift.z, 0, Time.deltaTime * pushBackResolve * 999);
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        move = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        {
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack + lift) * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bulletFlash, bulletSpawn.transform.position, transform.rotation);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            IDamage damagable = hit.collider.GetComponent<IDamage>();
            if (damagable != null)
            {
                damagable.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void giveHealthPoints(int amount)
    {
        healthPoints += amount;
    }

    public void takeDamage(int amount)
    {
        healthPoints -= amount;
        if(healthPoints <= 0)
        {
            gamemanager.instance.youLoseMenu();
        }
    }
    public void spawnPlayer()
    {
        healthPoints = startHealth;
        controller.enabled = false;
        transform.position = gamemanager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }
    public void physics(Vector3 push)
    {
        lift += push;
    }
    public void addPlayerSeed(int amount)
    {
        playerSpeed += amount;
    }
    public void addPlayerDamage(int amount)
    {
        shootDamage += amount;
    }

    public void addPlayerJumps(int amount)
    {
        jumpsMax += amount;
    }

    public void addPlayerHealth(int amount)
    {
        healthPoints += amount;
    }
}
