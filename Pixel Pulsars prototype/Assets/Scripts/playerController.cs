using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour, IDamage, IPhysics
{
    [Header("--- Components ---")]
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject bulletSpawn;
    [SerializeField] GameObject bulletFlash;
    [SerializeField] GameObject gunModel;

    [Header("--- Player Stats ---")]
    public int healthPoints;
    public float playerSpeed;
    public float runSpeed;
    public int jumpsMax;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int pushBackResolve;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float leanSpeed;
    [SerializeField] float leanMaxAngle;
    [SerializeField] Vector3 pushBack;
    [SerializeField] int maxStamina;



    //Variable Defintions: 
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpedTimes;
    private bool isShooting;
    private float currentLeanAngle = 0f;
    private int startHealth;
    private int pushBackResTemp;
    private float baseSpeed = 6;
    private bool isSprinting;
    private ParticleSystem gunshotEffect;



    private void Start()
    {
        startHealth = healthPoints;
        pushBackResTemp = pushBackResolve;
        gamemanager.instance.playerScript.spawnPlayer();

    }

    void Update()
    {
        movement();
        //lean();
        Sprint();
        StaminaRegen();

        if (Input.GetButtonDown("Shoot") && !isShooting)
        {
            gunshotEffect.Play();
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
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
    }

    void Sprint()
    {
       float basePlayerSpeed = playerSpeed;

       if (Input.GetKey(KeyCode.LeftShift))
       {
            setPlayerSpeed(runSpeed);
            isSprinting = true;
       }
       else
       {
            setPlayerSpeed(baseSpeed);
            isSprinting = false;
       }
    }

    void StaminaRegen()
    {
        float playerStamina = maxStamina;

        if (!isSprinting) 
        {
            if (playerStamina <= maxStamina - 0.01)
            {
                playerStamina += 10 * Time.deltaTime;
            }
        }
        if (isSprinting)
        {
            if (playerStamina > 0.01)
            {
                playerStamina -= 50 * Time.deltaTime;
            }
            if (playerStamina <= 0)
            {
                playerSpeed = baseSpeed;
                isSprinting = false;
            }
        }
        
    }
    
    IEnumerator shoot()
    {
        isShooting = true;
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
        gunshotEffect.Stop();
    }

    public void giveHealthPoints(int amount)
    {
        healthPoints += amount;
        updatePlayerUI();
    }

    public void takeDamage(int amount)
    {
        healthPoints -= amount;
        updatePlayerUI();

        if (healthPoints <= 0)
        {
            gamemanager.instance.youLoseMenu();
        }
    }

    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)healthPoints / startHealth;
    }

    public void spawnPlayer()
    {
        healthPoints = startHealth;
        updatePlayerUI();
        controller.enabled = false;
        transform.position = gamemanager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }
    public void physics(Vector3 push)
    {
        pushBack += push;
    }
    public void addPlayerSeed(float amount)
    {
        playerSpeed += amount;
    }
    public void setPlayerSpeed(float amount)
    {
        playerSpeed = amount;
    }
    public void addPlayerDamage(int amount)
    {
        shootDamage += amount;
    }
    public void setPlayerDamage(int amount)
    {
        shootDamage = amount;
    }
    public void addPlayerJumps(int amount)
    {
        jumpsMax += amount;
    }
    public void setPlayerJumps(int amount)
    {
        jumpsMax = amount;
    }

    public void addPlayerHealth(int amount)
    {
        healthPoints += amount;
    }
    public void setPlayerHealth(int amount)
    {
        healthPoints = amount;
    }
    public void airLiftToggle()
    {
        if(pushBack.y == 0)
        {
            pushBackResolve = 0;
        }
        if(pushBack.y != 0)
        {
            pushBackResolve = pushBackResTemp;
        }
    }
    public void setGunModel(gun model)
    {
        GameObject spawnedGun = Instantiate(model.model, gunModel.transform.position, gunModel.transform.rotation);
        spawnedGun.transform.parent = gunModel.transform;
        gunshotEffect = gunModel.GetComponentInChildren<ParticleSystem>();
        shootDamage = model.shootDamage;
        shootDistance = model.shootDistance;
        shootRate = model.shootRate;
    }
}
