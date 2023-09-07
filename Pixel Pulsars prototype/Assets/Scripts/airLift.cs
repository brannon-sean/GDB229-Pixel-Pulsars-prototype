using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airLift : MonoBehaviour
{
    [SerializeField] float liftStrength;
    Vector3 sender = Vector3.zero;

    private void OnTriggerStay(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }

        IPhysics lift = other.GetComponent<IPhysics>();
        if(lift != null)
        {
            lift.physics((other.transform.position - transform.position).normalized * liftStrength);
        }
    }
}
