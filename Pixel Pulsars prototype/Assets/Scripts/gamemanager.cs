using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [Header("--- Player Components ---")]
    public GameObject player;
    public playerController playerScript;
    public List<Image> inventoryItems;
    public List<Image> abilitySlots;
    public GameObject playerSpawnPos;
    public List<character> characterList;
    public List<gun> gunList;

    [Header("--- UI Components ---")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject storeMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject fullMainMenu;
    [SerializeField] GameObject characterSelectionMenu;
    public GameObject activeMenu;

    [Header("--- Enemy Components ---")]
    [SerializeField] int enemiesRemain;

    [Header("--- Store Components ---")]
    [SerializeField] List<Item> possibleItems;
    public List<Item> storeItems;
    public List<GameObject> storeCards;
<<<<<<< Updated upstream

    public List<character> characterList;
    public List<gunStats> gunList;

    // UI Changes
    public Image playerHPBar;
    [SerializeField] GameObject playerDamageFlash;
    [SerializeField] TMP_Text enemiesRemainingText;
    [SerializeField] TMP_Text currentWaveRemainingText;


=======
    public Item coin;
>>>>>>> Stashed changes

    bool isPaused;
    bool pickup;


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        Cursor.lockState = CursorLockMode.Confined;
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

    public void toggleCharacterSection(bool state)
    {
        if (state)
        {
            characterSelectionMenu.SetActive(state);
        }
        else
        {
            stateUnpause();
            toggleFullMainMenu(false);
        }
    }
    public void toggleFullMainMenu(bool state)
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu = null;
        isPaused = !isPaused;
        fullMainMenu.SetActive(state);
        stateUnpause();
    }
    public void toggleMainMenu(bool state)
    {
        if (state)
        {
            mainMenu.SetActive(state);
            activeMenu = mainMenu;
            statePause();
        }
        else
        {
            stateUnpause();
            toggleCharacterSection(true);
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

        enemiesRemainingText.text = enemiesRemain.ToString("0");

        if(enemiesRemain <= 0)
        {
            StartCoroutine(youWinMenu());
        }
    }
    public void updateStoreMenu()
    {
        storeCards[0].SetActive(true);
        storeCards[1].SetActive(true);
        storeCards[2].SetActive(true);
        for (int j = 0; j < storeItems.Count; j++)
        {
            int random = Random.Range(0, possibleItems.Count);
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

    public IEnumerator playerFlashDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }
}