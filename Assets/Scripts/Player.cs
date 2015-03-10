using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

    // player's damage output
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 10;
    public float restartLevelDelay = 1f;
    public Text foodText;

    private GameObject mainCam;
    private Animator animator;
    private int food;

	// Use this for initialization
	protected override void Start () {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food " + food;
        mainCam = GameObject.Find("Main Camera");
        base.Start();
	}
	
    private void OnDisable()
    {
        // store points when changing levels
        GameManager.instance.playerFoodPoints = food;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // hunger clock
        food--;
        foodText.text = "Food " + food;

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

        horisontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // prevent diagonal movement from axis
        //if (horisontal != 0)
        //    vertical = 0;

        // Take regular movement also from keypad
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            vertical = -1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
             horisontal = -1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            horisontal = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            vertical = 1;
        }
        
        // take diagonal movement from corner keys

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.End))
        {
            horisontal = -1;
            vertical = -1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.PageDown))
        {
            horisontal = 1;
            vertical = -1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Home))
        {
            horisontal = -1;
            vertical = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.PageUp))
        {
            horisontal = 1;
            vertical = 1;
        }
        if (horisontal != 0 || vertical != 0)
        {
            AttemptMove<Walls>(horisontal, vertical);
           // mainCam.GetComponent<CameraManager>().updatePosition((int)transform.position.x + horisontal, (int)transform.position.y + vertical);

        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } 
        else if (other.tag == "Food")
        {

            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: "+ food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;

            other.gameObject.SetActive(false);
        }

    }

    protected override void OnCantMove<T>(T component)
    {
        Walls hitWall = component as Walls;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
        
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();

    }
    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager.instance.GameOver();
    }


}
