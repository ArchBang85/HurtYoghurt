﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

    // player's damage output
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 10;
    public float restartLevelDelay = 1.5f;
    public Text foodText;

    private BoardManager BoardManager;
    private GameObject gM;

    private GameObject mainCam;
    private Animator animator;
    private int food;

    // 
    void Awake()
    {

    }

	// Use this for initialization
    protected override void Start () {
        //animator = GetComponent<Animator>();
        //food = GameManager.instance.playerFoodPoints;
        //foodText.text = "Food " + food;
        mainCam = GameObject.Find("Main Camera");
        gM = GameObject.FindGameObjectWithTag("GameController");
        BoardManager = gM.GetComponent<BoardManager>();
        base.Start();
	}
	
    private void OnDisable()
    {
        // store points when changing levels
        //GameManager.instance.playerFoodPoints = food;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();
        GameManager.instance.playerTurn = false;


    }



    	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.playerTurn) return;
        int horisontal = 0;
        int vertical = 0;

        //horisontal = (int)Input.GetAxisRaw("Horizontal");
        //vertical = (int)Input.GetAxisRaw("Vertical");

        // prevent diagonal movement from axis
        //if (horisontal != 0)
        //    vertical = 0;

        // Skip turn with x or 5
        if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.X))
        {
            GameManager.instance.playerTurn = false;
            CheckIfGameOver();
        }

        // Take regular movement also from keypad
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.S))
        {
            vertical = -1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.A))
        {
             horisontal = -1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.D))
        {
            horisontal = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.W))
        {
            vertical = 1;
        }
        
        // take diagonal movement from corner keys

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.End) || Input.GetKeyDown(KeyCode.Z))
        {
            horisontal = -1;
            vertical = -1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.C))
        {
            horisontal = 1;
            vertical = -1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Q))
        {
            horisontal = -1;
            vertical = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.E))
        {
            horisontal = 1;
            vertical = 1;
        }


        if ((horisontal != 0 || vertical != 0) && !(horisontal !=0 && vertical != 0))
        {
            //AttemptMove<Walls>(horisontal, vertical);
            if(BoardManager.checkMainDirections(horisontal, vertical, this.transform.position))
            {


                Vector3 end = new Vector3((int)horisontal, (int)vertical, 0) + new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0);
                StartCoroutine(SmoothMovement(end));
                CheckIfGameOver();
                mainCam.GetComponent<CameraManager>().updatePosition((int)transform.position.x + (int)horisontal, (int)transform.position.y + (int)vertical);
                GameManager.instance.playerTurn = false;

                BoardManager.updatePlayerPosition(new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0), new Vector3((int)horisontal, (int)vertical, 0) + new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0));

            }
            //this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0), 0f);
        }

        // UGLY HACK
        else if (horisontal != 0 && vertical != 0)
        {
            
            //BoardManager = gM.GetComponent<BoardManager>();
            // Different movement for diagonals so that can squeeze by corners, BUT shouldn't get out of monastery
            // Check for full-stack yoghurt and walls and presence in monastery
            if(BoardManager.checkDiagonal(horisontal, vertical, this.transform.position))
            {
               // this.GetComponent<BoxCollider2D>().enabled = false;
                //this.transform.collider2D.enabled = false;
                Vector3 end = new Vector3((int)horisontal, (int)vertical, 0) + new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0);
                StartCoroutine(SmoothMovementThroughWalls(end));
                CheckIfGameOver();
                mainCam.GetComponent<CameraManager>().updatePosition((int)transform.position.x + (int)horisontal, (int)transform.position.y + (int)vertical);
                GameManager.instance.playerTurn = false;

                BoardManager.updatePlayerPosition(new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0), new Vector3((int)horisontal, (int)vertical, 0) + new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0));
              
            }
                       
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        // How to handle diagonal movement here?

        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } 

        else if (other.tag == "Potash")
        {
            BoardManager.potashCount += 1;
            BoardManager.potashText.GetComponent<Text>().text = BoardManager.potashCount.ToString();
            other.gameObject.SetActive(false);

            int r = Random.Range(0, 10);

            if(r<3)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You take the bag of potash and hang it off your belt.");
            } else if (r<6) {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You grab the small bag of potash and hope it will be of some use.");
            } else {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You take the potash, you know yoghurt  doesn't like alkali.");
            }
            
        }
        else if (other.tag == "Acid")
        {
            BoardManager.acidCount += 1;
            BoardManager.acidText.GetComponent<Text>().text = BoardManager.acidCount.ToString();
            other.gameObject.SetActive(false);

            int r = Random.Range(0, 10);

            if (r < 3)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You pick up the container of acid.");
            }
            else if (r < 6)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You grab the acid, taking care not to spill too much of it on yourself.");
            }
            else
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You take the acid. The yoghurt likes to grow in acidic areas.");
            }

        }
        else if (other.tag == "Relic")
        {
            BoardManager.relicCount += 1;
            BoardManager.relicText.GetComponent<Text>().text = BoardManager.relicCount.ToString();
            gM.GetComponent<GameManager>().relics += 1;
            other.gameObject.SetActive(false);


            int r = Random.Range(0, 14);

            if (r < 1)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You hold the ancestor's revered face.  It hums.");
            }
            else if (r < 2)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You remember the tales this head has told you over the years, you hold on to it tight.");
            }
            else if (r < 3)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("This face has been in the tree for seventeen generations, listening.");
            }
            else if (r < 4)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("This relic knows almost all of your youthful embarrassements and fleeting loves.");
            }
            else if (r < 5)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("This foremother told you of distant lands and men with iridescent feathers.");
            }
            else if (r < 6)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("The relic's bronze eyes shine comfort towards you as you pocket it.");
            }
            else if (r < 7)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("You'd almost forgotten the existence of this relic, but its beauty strikes you now.");
            }
            else if (r < 8)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("Why do you disturb me? The figurine hisses.");
            }
            else if (r < 9)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("'If the yoghurt spreads on a caustic mix the walls might come down!'");
            }
            else if (r < 10)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("'What is it, child?' the metal head asks as you grab it.");
            }
            else if (r < 11)
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("'Put me down! I long for the sweet night.'");
            }
            else
            {
                BoardManager.gameObject.GetComponent<LogManager>().logMessage("'Block doorways with potash!' The elder barks from your clutches.");
            }
        }

    }

    protected override void OnCantMove<T>(T component)
    {
        Walls hitWall = component as Walls;
        hitWall.DamageWall(wallDamage);
       // animator.SetTrigger("playerChop");
        
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);


    }

    public void LoseFood (int loss)
    {
       // animator.SetTrigger("playerHit");
        //food -= loss;
        //foodText.text = "-" + loss + " Food: " + food;
        //CheckIfGameOver();

    }
    private void CheckIfGameOver()
    {
        //Debug.Log("Checking for game over");
        if (BoardManager.checkPlayerSurrounded(new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0)))
        {
            gM.GetComponent<GameManager>().GameOver();
            
            Debug.Log("game over");
        }

    }


}
