using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;

    private int jumpedTimes;
    [SerializeField] int jumpedMax;

    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Vector3 move;
    private bool isShooting;

    private void Start()
    {
        
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jumpedTimes = 0;
            playerVelocity.y = 0f;
        }

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        move = Input.GetAxis("Horizontal") * transform.right +
               Input.GetAxis("Vertical") * transform.forward;

        controller.Move(move * Time.deltaTime * playerSpeed);
                

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpedTimes < jumpedMax)
        {
            jumpedTimes++;
            playerVelocity.y += jumpHeight;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Update()
    {
        movement();

        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            
            if(damageable != null)
            {
                damageable.takeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }
}
