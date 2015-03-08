using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public GameObject floorCube;


	// Use this for initialization
	void Start () {
	    for (int i = 0; i < 15; i++)
        {
            Instantiate(floorCube, new Vector3(i * floorCube.transform.localScale.x + 1.5f, 0, 0), Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
