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
    private bool doingSetup;


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

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        // Wait start time before hiding image
        Invoke("HideLevelImage", levelStartDelay);

        // Clear out from last level
        enemies.Clear();
        boardScript.SetupScene(level);
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

        if (playerTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());



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
