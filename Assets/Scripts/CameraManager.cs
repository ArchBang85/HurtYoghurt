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
    
    // Use this for initialization
	void Start () {
        Debug.Log(startPos);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            

            if (Camera.main.orthographicSize <= orthographicSizeMin)
            {
                // if camera is zoomed in then keep it centered on the character
                if(Camera.main.orthographicSize > closerSizeMax)
                {
                    Camera.main.orthographicSize = closerSizeMax;
                }
                else
                {
                    if (Camera.main.orthographicSize > closerSizeMin)
                    {
                        Camera.main.orthographicSize -= 1;
                    }
                }

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
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
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
