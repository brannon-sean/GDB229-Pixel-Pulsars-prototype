using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [Header("--- Components ---")]
    [SerializeField] Rigidbody rigidBody;

    [Header("--- Bullet Stats ---")]
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] float destroyTime;

    void Start()
    {
        rigidBody.velocity = (gamemanager.instance.player.transform.position - transform.position) * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damagable = other.GetComponent<IDamage>();
        if (damagable != null)
        {
            damagable.takeDamage(damage);
        }

        Destroy(gameObject);
    }

}
