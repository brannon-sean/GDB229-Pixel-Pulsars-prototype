using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    [SerializeField] Item item;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerInventory inventory = gamemanager.instance.player.GetComponent<playerInventory>();
            inventory.addItem(item);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gamemanager.instance.togglePickup();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            gamemanager.instance.togglePickup();
        }
    }
}
