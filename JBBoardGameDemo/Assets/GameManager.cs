using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //This is going to be a singleton - a special design pattern which basically allows it to be equal to an objet
    public static GameManager Instance { private set; get; }
    [Header("Panels")][SerializeField] private GameObject pregamePanel;
    [SerializeField] private CurrentTurnIndicator turnIndicator;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private DieRoller rollMenu;
    [SerializeField] private GameObject alphaPopUp;

    [Header("Misc")]
    [SerializeField] private ObjectSelector objectSelector;
    [SerializeField] private CurrentPlayer itemTitle;
    [SerializeField] private CurrentPlayer itemInventory;
    [SerializeField] private CurrentPlayer dragonTitle;
    [SerializeField] private CurrentPlayer dragonInventory;
    [SerializeField] private GameObject playerpiece;

    private int numberOfPlayers;
    private DieRoller dice;

    //Note:    I'm on 42:56 of Piece Placement and Turns rn  and we want to make sure
    //      we implement a way to disable the  Object Selector  script on the main camera while
    //      the pregame setup screen is up. Because as of rn we can interact with the game through the screen
    //So we would make a serialized field for the Main camera, "objectSelector"  so that we can set objectSelector.enabled = false (@46:58)
    //and we've gotta turn it back on (to True) later, as well
    private int currentPlayer = -1;
    private int fiercePoints = 0;
    private int docilePoints = 0;
    private int cunningPoints = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //While the pre-game panel screen is up, disable all game features
        if(objectSelector != null)
        {
            objectSelector.enabled = false;
        }

        if(turnIndicator != null)
        {
            turnIndicator.gameObject.SetActive(false);
        }
        if (gameUI != null)  //Keep in mind we named the End Turn button  "gameUI"
        {
            gameUI.SetActive(false);
        }
        if (winScreen != null)  //Keep in mind we named the End Turn button  "gameUI"
        {
            winScreen.SetActive(false);
        }
        if (rollMenu != null)
        {
            rollMenu.gameObject.SetActive(false);
        }
        if (alphaPopUp != null)
        {
            alphaPopUp.SetActive(false);
        }

        if (pregamePanel != null) 
        {
            pregamePanel.SetActive(true);
        }
        

    }

    public void SetNumberOfPlayers(string players)
    {
        try
        {
            numberOfPlayers = int.Parse(players);
        }

        catch (UnityException e)
        {
            numberOfPlayers = 0;
        }
        for (int i = 0; i < numberOfPlayers; i++) {
            Instantiate(playerpiece, new Vector3((float)-32.69, (float)0.3, (float)-17.96), Quaternion.identity);
        }
        Debug.Log($"Number of Players set to {numberOfPlayers}");


    }


    public void StartGame()
    {
        if (numberOfPlayers > 0)
        {
            
            pregamePanel.SetActive(false);
            Debug.Log("Game has started");
            objectSelector.enabled = false;
            currentPlayer = Random.Range(0, numberOfPlayers);
            turnIndicator.SetText($"It's Player {currentPlayer + 1}'s Turn");
            turnIndicator.gameObject.SetActive(true);

            //I can set dragons and items to none and 0 here respectively.

        }
    }

    public void HandleTurn()
    {
        Debug.Log("Turn begins!");
        objectSelector.enabled = true;
        turnIndicator.gameObject.SetActive(false);
        gameUI.SetActive(true);
        itemInventory.SetTitle($"Player {currentPlayer + 1}'s Items:");
        dragonInventory.SetTitle($"Player {currentPlayer + 1}'s Items:");

    }

    //Dice roll  -->   result = 1 + rand(6);  while item generation is  1 + rand(8)
    public void Encounter()
    {
        Debug.Log("I made it back into Game Manager and Taming should commence");
        objectSelector.enabled = false; //Stop movement


        int alphaChance = -1;
        alphaChance = 1 + Random.Range(0, 10);

        Debug.Log($"alphaChance is: {alphaChance}");
        if (alphaChance == 1 || alphaChance == 2 || alphaChance == 3)
        {
            alphaPopUp.SetActive(true);
        }
        else
        {
            RegularEncounter();
        }
   
    }

    public void AlphaEncounter()
    {
        Debug.Log("We're in the Alpha Encounter funciton");
        alphaPopUp.SetActive(false);
        rollMenu.gameObject.SetActive(true);  //bring up the roll menu
        int tameDice;
        bool tameSuccess = false;

        //I also have to think about alpha enocounters


        tameDice = rollMenu.Roll("Taming");
        if (tameDice > 5)
        {
            tameSuccess = true;
        }
        if(tameSuccess)
        {
            fiercePoints = fiercePoints + 2;
            docilePoints = docilePoints + 2;
            cunningPoints = cunningPoints + 2;
            Debug.Log("I FUCKIN GOT IT!!");
        }
        else
        {
            Debug.Log("No alpha");
        }

        dragonInventory.SetInventory($"- Fierce: ({fiercePoints})\r\n- Cunning: ({cunningPoints})\r\n- Docile ({docilePoints})");

    }

    public void RegularEncounter()
    {
        alphaPopUp.SetActive(false);
        rollMenu.gameObject.SetActive(true);  //bring up the roll menu

        int dragonType;
        int tameDice;
        bool tameSuccess = false;

        //I also have to think about alpha enocounters
         
        
        dragonType = rollMenu.Roll("Encounter");
        Debug.Log($"Dragon Type is: {dragonType}");
        tameDice = rollMenu.Roll("Taming");
        Debug.Log($"Dice Roll is: {tameDice}");
        if (tameDice >= 3)
        {
            tameSuccess = true;
        }

        if (tameSuccess)
        {
            if (dragonType <=2)
            {
                docilePoints = docilePoints + 2;
                Debug.Log("Docile Dragon Tamed!");
            }
            if ( 2<dragonType && dragonType<5)
            {
                cunningPoints = cunningPoints + 2;
                Debug.Log("Cunning Dragon Tamed!");
            }
            if (dragonType >= 5)
            {
                fiercePoints = fiercePoints + 2;
                Debug.Log("Fierce Dragon Tamed!");
            }

        }
        dragonInventory.SetInventory($"- Fierce: ({fiercePoints})\r\n- Cunning: ({cunningPoints})\r\n- Docile ({docilePoints})");

    }


    public void EndTurn()
    {
        Debug.Log("Turn ends");
        objectSelector.enabled = true;
        currentPlayer++;
        if (currentPlayer >= numberOfPlayers)
        {
            currentPlayer = 0;
        }
        turnIndicator.SetText($"It's Player {currentPlayer + 1}'s Turn");
        turnIndicator.gameObject.SetActive(true);
        gameUI.SetActive(false);
        rollMenu.gameObject.SetActive(false);

        if(fiercePoints > 8 || docilePoints > 8 || cunningPoints > 8)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
        objectSelector.enabled = false;
        gameUI.SetActive(false);
        rollMenu.gameObject.SetActive(false);
        alphaPopUp.SetActive(false);
        winScreen.SetActive(true);


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
