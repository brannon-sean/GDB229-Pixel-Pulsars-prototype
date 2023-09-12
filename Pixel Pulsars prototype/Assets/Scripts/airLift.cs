using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airLift : MonoBehaviour
{
    [SerializeField] float liftStrength;
    

    private void OnTriggerStay(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }



        IPhysics lift = other.GetComponent<IPhysics>();
        if(lift != null)
        {
           lift.physics(other.transform.up + Vector3.up * liftStrength);
        }
        gamemanager.instance.playerScript.airLiftToggle();
    }
    private void OnTriggerExit(Collider other)
    {
        gamemanager.instance.playerScript.airLiftToggle();
    }
}
