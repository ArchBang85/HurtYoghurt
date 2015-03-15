using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;

    public float turnDelay = .1f;

    private int turnTicks = 100;
    public int turnTickReset = 100;
    public int turnTickCost = 100;

    public static GameManager instance = null;
    public BoardManager boardScript;
    public VesicaPisces piscesScript;
    private int level = 1;
    public float relics = 0;
    public int playerFoodPoints = 100;
    [HideInInspector]public bool playerTurn = true;

    private List<Enemy> enemies;
    private bool enemiesMoving;
    private Text levelText;
    private GameObject levelImage;
    private GameObject CharSelectionImage;

    private bool doingSetup;
    private bool charSetup;

    private int Age = 1;
    private int Condition = 1;
    private int Rank = 1;

    private bool GameOverFlag = false;
    public bool canRestart = false;
    public bool doSetup = true;
    public GameObject readyButton;
    public GameObject[] ageOptions = new GameObject[3];
    public GameObject[] conditionOptions = new GameObject[3];
    public GameObject[] rankOptions = new GameObject[3];
    private int optionIndex = -1000;
    private int optionType = 0;
    // Storing the player's selections
    private int[] characterCharacteristics = new int[3];

    public GameObject charSelectToggle;


	// Use this for initialization
	void Awake () {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // retain manager on scene load
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();

        InitGame();
        //doSetup = true;

    }

    void Start()
    {
       
        
    }


    // Call when scene is loaded
    private void OnLevelWasLoaded(int index)
    {
        if (level < 11)
        {
            level++;
        }
        if(level >= 11 && level < 13)
        {

            //InitGame();
            // Game won!
            //InitGame();
            GameWon();
            
           // this.GetComponent<LogManager>().logMessage("You've made it out of the tree alive! You carry with you " + relics + " relics.");

        }
        else if (level > 11)
        {
            return;
            // something's awry
            //GameWon();
        } else 
        {


            InitGame();
        }




    }

    void InitGame()
    {
        if (doSetup)
        {
            doingSetup = true;
            charSetup = true;

            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            HideLevelImage();

            // Character select screen
            CharSelectionImage = GameObject.Find("CharSelectionImage");
            CharSelectionImage.SetActive(true);
            charSelectToggle = GameObject.Find("CharSelectToggle");
            ageOptions[0] = GameObject.Find("AgeOption1");
            ageOptions[1] = GameObject.Find("AgeOption2");
            ageOptions[2] = GameObject.Find("AgeOption3");
            conditionOptions[0] = GameObject.Find("ConditionOption1");
            conditionOptions[1] = GameObject.Find("ConditionOption2");
            conditionOptions[2] = GameObject.Find("ConditionOption3");
            rankOptions[0] = GameObject.Find("RankOption1");
            rankOptions[1] = GameObject.Find("RankOption2");
            rankOptions[2] = GameObject.Find("RankOption3");

          
        }

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        readyButton = GameObject.Find("ReadyText");
        // Get a component ref to the attached script
        boardScript = GetComponent<BoardManager>();
        piscesScript = GameObject.Find("PiscesGenerator").GetComponent<VesicaPisces>();
        // Clear out from last level
        enemies.Clear();

        piscesScript.setupPisces();

        boardScript.SetupScene(level);
        //if (!GameOverFlag)
            ShowLevelImage();
    }

    private void ShowLevelImage()
    {
        /*
        if(CharSelectionImage.active)
        {
            CharSelectionImage.SetActive(false);
        }
        */
        if(level < 2)
        {
            levelText.text = "You are at the top of the tree\n\nHurt Yoghurt";
        }
        else
        {
            levelText.text = "You are " + level + " levels down the tree\n\nHurt Yoghurt";
        }
        levelImage.SetActive(true);

        // Wait start time before hiding image
        Invoke("HideLevelImage", levelStartDelay);
    }

    private void ShowLevelImageWithText(string s)
    {

        levelText.text = s;

        levelImage.SetActive(true);

        // Wait start time before hiding image
        Invoke("HideLevelImage", levelStartDelay);
    }



    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        //levelImage = GameObject.Find("LevelImage");
        //levelText = GameObject.Find("LevelText").GetComponent<Text>();
        //GameOverFlag = true;
        levelText.text = "On story " + level + ", you were fermented and became one with yoghurt, "+ relics + " relics clutched in your arms, perishing.";
        levelImage.SetActive(true);
        enabled = false;
        
        canRestart = true;
       // StartCoroutine("RestartGame", 6.0f);
    }

    public void GameWon()
    {
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        if(relics==0)
        {
            levelText.text = "You made it out of the tree of life alive, but did not rescue any relics. Memories of your cowardice will be etched into mountains.";
        }
        else if (relics < 9)
        {
            levelText.text = "You made it out of the tree of life alive, carrying with you " + relics + " relics. So much has still been lost.";
        }
        else if (relics < 20)
        {
            levelText.text = "Covered in yoghurt, you stumble out of the monastery. You manager to rescue " + relics + " relics. More could barely have been expected of you.";
        }
        else if (relics < 35)
        {
            levelText.text = "You emerge from the tree triumphant, carrying with you " + relics + " invaluable relics. The stories will live on.";
        }
        else 
        {
            levelText.text = "You reach the soil with " + relics + " relics in your arms. Your people weep with joy. Your memory will echo through the ages.";
        }
    
        levelImage.SetActive(true);
       // GameOverFlag = true;
        enabled = false;
        level = 0;
        relics = 0;

        //StartCoroutine("RestartGame", 15.0f);
    }

    void RestartGame()
    {
        level = 0;
        relics = 0;
        GameOverFlag = false;

        if (Application.loadedLevel == 0)
        { 
            Application.LoadLevel(1);
        }
        else
        {    
            Application.LoadLevel(0);
        }
    }

	// Update is called once per frame
	void Update () {


        //  if (canRestart)
       // {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("restarting");
            //canRestart = false;
            level = 0;
            relics = 0;
            if (Application.loadedLevel == 0)
            {
              
                Application.LoadLevel(1);

            }
            else
            {
               
                Application.LoadLevel(0);

            }

        }

        if (GameOverFlag)
        {
            return;
        }
        //}

        if (piscesScript == null)
        {
            piscesScript = GameObject.Find("PiscesGenerator").GetComponent<VesicaPisces>();

        }
        // Setup the game
        if(charSetup)
        {
            charSetupUI();
        }

        if (playerTurn || enemiesMoving || doingSetup || charSetup)
            return;

        yoghurtTurn();

        // StartCoroutine(MoveEnemies());

	}

    void charSetupUI()
    {
        GameObject optionChosen;
        int xDir = 0;
        int yDir = 0;
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            yDir = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            yDir = -1;
        }

        optionIndex += yDir;
        if (optionIndex >= 4)
            optionIndex = 0;
        if (optionIndex < 0)
            optionIndex = 2;

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            xDir = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            xDir = -1;
        }

        optionType += xDir;
        if (optionType >= 3)
            optionType = 0;
        if (optionType < 0)
            optionType = 2;

        if (yDir != 0 || xDir != 0)
        {
            moveToggle(optionType, optionIndex);
        }

        // Action selection
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            charOptionSelect(optionType, optionIndex);

            // Store player's selection in array
            characterCharacteristics[optionType] = optionIndex;
        }
    }

    void moveToggle(int optionType, int optionIndex)
    {
        GameObject optionChosen = null;
        Debug.Log(optionIndex);


        if (optionIndex < 3)
        {
            if (optionType == 0)
            {
                optionChosen = ageOptions[optionIndex];
            }
            else if (optionType == 1)
            {
                optionChosen = conditionOptions[optionIndex];
            }
            else
            {
                optionChosen = rankOptions[optionIndex];
            }
            // Arbitrary numbers buffering this, beware!
            charSelectToggle.transform.position = new Vector3(optionChosen.transform.position.x - 110, optionChosen.transform.position.y - 50, 0.0f);

        }


        // Move to ready button
        if (optionIndex == 3)
        {
            optionChosen = readyButton;
            charSelectToggle.transform.position = new Vector3(optionChosen.transform.position.x - 110, optionChosen.transform.position.y, 0.0f);
        }


    }

    void charOptionSelect(int optionType, int optionIndex)
    {
        GameObject optionChosen = null;
        int[] notChosen = new int[2];
        if(optionIndex == 0)
        {
            notChosen[0] = 1;
            notChosen[1] = 2;
        } else if (optionIndex == 1)
        {
            notChosen[0] = 0;
            notChosen[1] = 2;
        } else 
        {
            notChosen[0] = 0;
            notChosen[1] = 1;
        }

        if (optionIndex < 3)
        { 
            if (optionType == 0)
            {
                optionChosen = ageOptions[optionIndex];
            }
            else if (optionType == 1)
            {
                optionChosen = conditionOptions[optionIndex];
            }
            else if (optionType == 2)
            {
                optionChosen = rankOptions[optionIndex];
            } 
        }

        // Start game if ready is selected
        if (optionIndex == 3)
        {
            charSetup = false;

            optionChosen = readyButton;
            // Colour chosen text
            optionChosen.GetComponent<Text>().color = new Color(0.235f, 0.5f, 0.235f, 1);
            // Italicise
            optionChosen.GetComponent<Text>().fontStyle = FontStyle.Italic;

            // Wait start time before showing level 
            Invoke("ShowLevelImage", levelStartDelay);
        }

        // Colour chosen text
        optionChosen.GetComponent<Text>().color = new Color(0.235f, 0.5f, 0.235f, 1); 
        // Italicise
        optionChosen.GetComponent<Text>().fontStyle = FontStyle.Italic;

        // Revert others in the same category
        for (int i = 0; i < 2; i++)
        {
            if (optionType == 0)
            {
                optionChosen = ageOptions[notChosen[i]];
            }
            else if (optionType == 1)
            {
                optionChosen = conditionOptions[notChosen[i]];
            }
            else
            {
                optionChosen = rankOptions[notChosen[i]];
            }
        
            // Colour normal
            optionChosen.GetComponent<Text>().color = new Color(0.9f, 0.9f, 0.9f, 1);
            // Font style normal
            optionChosen.GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void yoghurtTurn()
    {

        boardScript.yoghurtBehaviour();
        turnTicks += turnTickCost;

        if (turnTicks > turnTickReset)
        {
            playerTurn = true;
            turnTicks = 0;
        }
        else
        {
            yoghurtTurn();
        }
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playerTurn = true;
        enemiesMoving = false;
    }
}
