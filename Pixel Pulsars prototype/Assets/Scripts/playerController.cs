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
    public float healthPoints;
    public float playerSpeed;
    public float runSpeed;
    public int jumpsMax;
    private float exhSpeed = 2;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int pushBackResolve;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float lifeSteal;
    [SerializeField] float leanSpeed;
    [SerializeField] float leanMaxAngle;
    [SerializeField] Vector3 pushBack;
    public float startHealth;
    [SerializeField] int totalStamina;
    public float playerStamina;
    private WaitForSeconds tickRate = new WaitForSeconds(0.1f);
    private bool isRegening;
    private bool isExhausted;

    //Variable Defintions: 
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpedTimes;
    private bool isShooting;
    private float currentLeanAngle = 0f;
    private int pushBackResTemp;
    private float baseSpeed = 6;
    private bool isSprinting;
    private ParticleSystem gunshotEffect;



    private void Start()
    {
        startHealth = healthPoints;
        playerStamina = totalStamina;
        pushBackResTemp = pushBackResolve;
        gamemanager.instance.playerScript.spawnPlayer();

    }

    void Update()
    {
        movement();
        //lean();
        Sprint();

        if (Input.GetButton("Shoot") && !isShooting)
        {
            try
            {
                gunshotEffect.Play();
            }
            catch
            {

            }
            
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
        if (playerStamina > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                setPlayerSpeed(runSpeed);
                isSprinting = true;

                if (isRegening == false)
                {
                    if (playerStamina > 0)
                    {
                        StartCoroutine(DrainStamina());
                    }
                    else if (playerStamina <= 0)
                    {
                        StopCoroutine(DrainStamina());
                        playerStamina = 0;
                    }
                    else
                    {
                        Debug.Log("isSprinting");
                    }
                }
                else if (isRegening == true)
                {
                    StopCoroutine(RegenStamina());
                    StartCoroutine(DrainStamina());
                }
            }
            else
            {
                StopCoroutine(DrainStamina());
                setPlayerSpeed(baseSpeed);
                isSprinting = false;

                if (playerStamina < totalStamina)
                {
                    StartCoroutine(RegenStamina());
                }
                else
                {
                    Debug.Log("!isSprinting");
                }

            }
        }
        else if (playerStamina <= 0)
        {
            StopCoroutine(RegenStamina());
            StartCoroutine(StaminaBreak());
            StopCoroutine(StaminaBreak());
        }
    }


    //void Stamina()
    //{
    //    if (playerStamina >= 0)
    //    {
    //            playerStamina -= 10 * Time.deltaTime;
    //            gamemanager.instance.playerStaminaBar.fillAmount = playerStamina / totalStamina;
    //    }
    //    else
    //    {
    //        
    //    }
    //}

    IEnumerator DrainStamina()
    {
        isRegening = false;

        while ((playerStamina >= 0) && isSprinting)
        {
            playerStamina -= 2 * Time.deltaTime;
            gamemanager.instance.playerStaminaBar.fillAmount = playerStamina / totalStamina;
            yield return tickRate;
        }
    }

    IEnumerator RegenStamina()
    {
        isRegening = true;
        while((playerStamina < totalStamina) && !isSprinting)
        {
            playerStamina += 2 * Time.deltaTime;
            gamemanager.instance.playerStaminaBar.fillAmount = playerStamina / totalStamina;
            yield return tickRate;
        }
    }

    IEnumerator StaminaBreak()
    {
        isExhausted = true;

        setPlayerSpeed(exhSpeed);
        yield return new WaitForSeconds(5);

        setPlayerSpeed(baseSpeed);
        playerStamina = totalStamina;
        gamemanager.instance.playerStaminaBar.fillAmount = playerStamina / totalStamina;

        isExhausted = false;
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
                if(startHealth >= healthPoints + (shootDamage * lifeSteal))
                {
                    healthPoints += shootDamage * lifeSteal;
                    updatePlayerUI();
                }
                else if(startHealth <= healthPoints + (shootDamage * lifeSteal))
                {
                    float healToFull = startHealth - healthPoints;
                    healthPoints += healToFull;
                    updatePlayerUI();
                }
                
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
    public void addShootRate(float amount)
    {
        shootRate -= amount;
    }
    public void setShootRate(float amount)
    {
        shootRate = amount;
    }

    public void addPlayerHealth(int amount)
    {
        healthPoints += amount;
        startHealth += amount;
    }
    public void setPlayerHealth(int amount)
    {
        healthPoints = amount;
    }
    public void addPlayerLifesteal(float amount)
    {
        lifeSteal += amount;
    }
    public void addPlayerShootDistance(int amount)
    {
        shootDistance += amount;
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
