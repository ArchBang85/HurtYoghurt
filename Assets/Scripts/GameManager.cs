using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;

    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    private int level = 1;
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

    public GameObject readyButton;
    public GameObject[] ageOptions = new GameObject[3];
    public GameObject[] conditionOptions = new GameObject[3];
    public GameObject[] rankOptions = new GameObject[3];
    private int optionIndex = 0;
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

        // Get a component ref to the attached script
	    boardScript = GetComponent<BoardManager>();

	    InitGame();
    }
	
    // Call when scene is loaded
    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();

    }

    void InitGame()
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
        readyButton = GameObject.Find("ReadyText");


     
        // Clear out from last level
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void ShowLevelImage()
    {

        if(CharSelectionImage.active)
        {
            CharSelectionImage.SetActive(false);
        }

        levelText.text = "Day " + level;
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
        levelText.text = "After " + level + " days, you starved to death.";
        levelImage.SetActive(true);
        enabled = false;
    }
	// Update is called once per frame
	void Update () {

        // Setup the game
        if(charSetup)
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

            if(yDir != 0 || xDir != 0)
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

        if (playerTurn || enemiesMoving || doingSetup || charSetup)
            return;
        StartCoroutine(MoveEnemies());

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
