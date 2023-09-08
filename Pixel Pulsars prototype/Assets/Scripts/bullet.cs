using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    [SerializeField] Rigidbody rigidBody;
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] float destroyTime;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody.velocity = transform.forward * speed;
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
