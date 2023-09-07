using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;

    [SerializeField] int healthPoints;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpsMax;
    [SerializeField] float gravityValue;

    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private int jumpedTimes;
    private bool isShooting;

    private void Start()
    {

    }

    void Update()
    {
        movement();

        if (Input.GetButtonDown("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    void movement()
    {
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
        controller.Move(playerVelocity * Time.deltaTime);
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
    }

    public void giveHealthPoints(int amount)
    {
        healthPoints += amount;
    }

    public void takeDamage(int amount)
    {
        healthPoints -= amount;
    }
}
