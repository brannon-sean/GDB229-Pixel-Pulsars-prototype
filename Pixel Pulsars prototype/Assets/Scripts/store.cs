using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class store : MonoBehaviour
{
    private bool storeEnabled;

    private void OnTriggerEnter(Collider other)
    {
        if (!storeEnabled)
        {
            gamemanager.instance.updateStoreMenu();
            gamemanager.instance.toggleStore(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (storeEnabled)
        {
            storeEnabled = true;
        }
    }
}
