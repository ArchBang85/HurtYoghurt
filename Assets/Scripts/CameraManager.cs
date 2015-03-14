using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public GameObject player;

    public int orthographicSizeMin = 15;
    public int orthographicSizeMax = 20;

    public int closerSizeMin = 4;
    public int closerSizeMax = 6;
    private int cameraCounter = 0;
    private Vector3 startPos = new Vector3(21, 14,0);

    public GameObject auxiliaryLog; // this gets controlled by the zoom level
    
    // Use this for initialization
	void Awake () {
        auxiliaryLog = GameObject.Find("AuxCanvas");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.Period)) 
        {
            

            if (Camera.main.orthographicSize <= orthographicSizeMin)
            {
       
                if(Camera.main.orthographicSize > closerSizeMax)
                {
                    // Leap to zoomed in level
                    Camera.main.orthographicSize = closerSizeMax;
                    // Make auxiliary text visible
                    auxiliaryLog.SetActive(true);
                }
                else
                {
                    if (Camera.main.orthographicSize > closerSizeMin)
                    {
                        Camera.main.orthographicSize -= 1;
                    }
                }
                // if camera is zoomed in then keep it centered on the character
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            }
            else
            {

                Camera.main.orthographicSize -= 1; 
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
            }
            
            if (Camera.main.orthographicSize < orthographicSizeMin)
            {

            }

        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Comma))
        {

            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, closerSizeMin, orthographicSizeMax);

            if (Camera.main.orthographicSize < closerSizeMax)
            {
                Camera.main.orthographicSize += 1;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, closerSizeMin, closerSizeMax);
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            }
            else if (Camera.main.orthographicSize == closerSizeMax)
            {
                // jump to higher level
                transform.position = new Vector3(startPos.x, startPos.y, -10);
                Camera.main.orthographicSize = orthographicSizeMin;
                // Make auxiliary text hidden
                auxiliaryLog.SetActive(false);

            }
            else
            {
                Camera.main.orthographicSize += 1;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
            }
            
        }
        
	}

    public void updatePosition(int x, int y)
    {
        // Camera zoomed in sufficiently for movement to be warranted
        if (Camera.main.orthographicSize < orthographicSizeMin)
        {   
            
            if (cameraCounter > 3)
                {
                    transform.position = new Vector3(x, y, -10);
                    cameraCounter = 0;
                }
           cameraCounter++;
        }
    }

}
