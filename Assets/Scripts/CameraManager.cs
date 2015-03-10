using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public GameObject player;

    public int orthographicSizeMin = 10;
    public int orthographicSizeMax = 15;

    public int closerSizeMin = 4;
    public int closerSizeMax = 9;

    private Vector3 startPos = new Vector3(21, 14,0);
    
    // Use this for initialization
	void Start () {
        Debug.Log(startPos);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            Camera.main.orthographicSize -= 1; //Mathf.Min(Camera.main.orthographicSize - 1, 6);
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, closerSizeMin, orthographicSizeMax);

            if (Camera.main.orthographicSize < orthographicSizeMin)
            {
                // if camera is zoomed in then keep it centered on the character
                
                transform.position =  new Vector3(player.transform.position.x , player.transform.position.y, -10);
            }

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            Camera.main.orthographicSize += 1;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, closerSizeMin, orthographicSizeMax);

            if (Camera.main.orthographicSize > 9)
            {
                // Centre camera
                transform.position = new Vector3(startPos.x, startPos.y, -10);

            }

        }
        
	}

    public void updatePosition(int x, int y)
    {
        if (Camera.main.orthographicSize < orthographicSizeMin)
        { 
            transform.position = new Vector3(x, y, -10);
        }
    }

}
