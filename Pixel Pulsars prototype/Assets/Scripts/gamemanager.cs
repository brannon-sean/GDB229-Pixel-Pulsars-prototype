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
    public GameObject playerSpawnPos;
    public List<character> characterList;
    public List<gun> gunList;
    [SerializeField] int experienceToNextLevel;

    [Header("--- UI Components ---")]
    public List<Image> inventoryItems;
    public List<Image> abilitySlots;
    public Item coin;
    public GameObject activeMenu;
    public Image playerHPBar;
    public Image playerStaminaBar;
    [SerializeField] Image levelBar;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    public GameObject loseMenu;
    [SerializeField] GameObject storeMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject fullMainMenu;
    [SerializeField] GameObject characterSelectionMenu;
    [SerializeField] GameObject playerDamageFlash;
    public TMP_Text waveTimerText;
    public TMP_Text waveNumberText;
    public TMP_Text stanimaText;
    public TMP_Text experienceText;
    public TMP_Text healthPointsText;

    [Header("--- Store Components ---")]
    public List<Item> storeItems;
    public List<GameObject> storeCards;
    [SerializeField] List<Item> possibleItems;

    [Header("--- Enemy Components ---")]
    [SerializeField] int enemiesRemain;


    //Variable definitions:
    public bool isPaused;
    bool pickup;
    private int experience;
    private int level;
    


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    private void Start()
    {
        addExperience(0);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePause();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
        else if (Input.GetButtonDown("Cancel") && activeMenu == storeMenu)
        {
            toggleStore(false);
        }
    }
    //Enter pause state
    public void statePause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }
    //Exit pause state
    public void stateUnpause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(isPaused);
        activeMenu = null;
    }
    //Toggle store menu and pause/unpause game.
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
    //Toggle character selection menu on start, unpause game once false
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
    //Deactivate main menu gameobject entirely.
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
    //Toggle main menu screen once character game is playing.
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
        activeMenu = loseMenu; 
        statePause();
        activeMenu.SetActive(isPaused);
    }
    public void updateGameGoal(int amount)
    {
        enemiesRemain += amount;

        //if (enemiesRemain <= 0)
        //{
        //    StartCoroutine(youWinMenu());
        //}
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
                //card.price.text = item.price.ToString();
                card.sprite.sprite = item.sprite;
            }
            i++;
        }
    }
    public void addExperience(int amount)
    {
        experience += amount;
        levelBar.fillAmount = (float)experience / experienceToNextLevel;
        experienceText.text = experience.ToString() + " / " + experienceToNextLevel.ToString();
        if (experience >= experienceToNextLevel)
        {
            level++;
            experienceToNextLevel = (int)(experienceToNextLevel * 1.3);
            levelUp();
        }
    }
    public void levelUp()
    {
        updateStoreMenu();
        toggleStore(true);
        experience = 0;
        levelBar.fillAmount = 0;
        playerScript.healthPoints = playerScript.startHealth;
        playerScript.updatePlayerUI();
    }

    public IEnumerator playerFlashDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }
}