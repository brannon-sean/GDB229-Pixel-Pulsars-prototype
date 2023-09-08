using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    public GameObject player;
    public playerController playerScript;
    public List<Image> inventoryItems;
    public List<Image> abilitySlots;
    public Item coin;

    public GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    //[SerializeField] GameObject pickupOption;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject storeMenu;

    [SerializeField] List<Item> possibleItems;

    public GameObject playerSpawnPos;

    [SerializeField] int enemiesRemain;
    public List<Item> storeItems;
    public List<GameObject> storeCards;

    

    bool isPaused;
    bool pickup;


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePause();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }else if (Input.GetButtonDown("Cancel") && activeMenu == storeMenu)
        {
            toggleStore(false);
        }
    }

    public void statePause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }

    public void stateUnpause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(isPaused);
        activeMenu = null;
    }
    //public void togglePickup(bool state)
    //{
    //pickup = state;
    //pickupOption.SetActive(pickup);
    //}
    public void toggleStore(bool state)
    {
        if (state)
        {
            storeMenu.SetActive(state);
            activeMenu = storeMenu;
            statePause();
        }
        else
        {
            stateUnpause();
        }
    }
    IEnumerator youWinMenu()
    {
        yield return new WaitForSeconds(1);
        statePause();
        activeMenu = winMenu;
        activeMenu.SetActive(isPaused);
    }
    public void youLoseMenu()
    {
        statePause();
        activeMenu = loseMenu;
        activeMenu.SetActive(isPaused);
    }
    public void updateGameGoal(int amount)
    {
        enemiesRemain += amount;
        if(enemiesRemain <= 0)
        {
            StartCoroutine(youWinMenu());
        }
    }
    public void updateStoreMenu()
    {
        for(int j = 0; j < 3; j++)
        {
            int random = Random.Range(0, 3);
            storeItems[j] = possibleItems[random];
        }
        int i = 0;
        foreach (Item item in storeItems)
        {
            if (item != null)
            {
               storeCard card = storeCards[i].GetComponent<storeCard>();
                card.title.text = item.itemName;
                card.description.text = item.description;
                card.price.text = item.price.ToString();
                card.sprite.sprite = item.sprite;
            }
            i++;
        }
    }
}