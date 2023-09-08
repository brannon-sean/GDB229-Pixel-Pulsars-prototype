using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    [SerializeField] Item item;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInventory inventory = gamemanager.instance.player.GetComponent<playerInventory>();
            inventory.addItem(item);
            Destroy(gameObject);
        }
    }
}
