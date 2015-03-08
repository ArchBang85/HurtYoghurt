using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public float xPos = 0;
    public float yPos = 0;
	// Use this for initialization
	void Awake () {

        if (GameManager.instance == null)
            Instantiate(gameManager, new Vector3(xPos, yPos, 0),Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
