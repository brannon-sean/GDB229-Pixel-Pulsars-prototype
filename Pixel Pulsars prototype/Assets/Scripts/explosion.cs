using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [Header("--- Explosion Stats ---")]
    [SerializeField] int explosionForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IPhysics lift = other.GetComponent<IPhysics>();
        if (lift != null)
        {
            lift.physics((other.transform.position - transform.position).normalized * explosionForce);
        }
    }
}
