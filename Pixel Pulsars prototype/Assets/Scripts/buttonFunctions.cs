using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpause();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void givePlayerHealthPoints(int amount)
    {
        gamemanager.instance.playerScript.giveHealthPoints(amount);
    }

    public void respawn()
    {
        gamemanager.instance.stateUnpause();
        gamemanager.instance.playerScript.spawnPlayer();
    }
    public void purchase(int card)
    {
        playerInventory inventory = gamemanager.instance.player.GetComponent<playerInventory>();
        if (inventory != null)
        {
            if (inventory.hasEnough(gamemanager.instance.coin, gamemanager.instance.storeItems[card].price))
            {
                inventory.addItem(gamemanager.instance.storeItems[card], 1);
                inventory.removeItem(gamemanager.instance.coin, 5);
                gamemanager.instance.storeCards[card].SetActive(false);
            }
            else 
            {
                Debug.Log("Player does not have enough");
            }
        }
    }
}
