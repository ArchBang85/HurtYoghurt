using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class LogManager : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject scrollLog;
    public List<string> logMessages;
    private string[] activeLogMessages = new string[5];
    public GameObject[] activeLogMessageStrips = new GameObject[5];
    public GameObject auxCanvas;
    public GameObject auxiliaryLog; // What to display as an overlay when zoomed in 
    public GameObject auxiliaryLog2; // What to display as an overlay when zoomed in 
    public bool zoomedIn = false;
    private int counter = 1;

    // Use this for initialization
    void Start()
    {
        scrollLog = GameObject.Find("ScrollLog");
        auxCanvas = GameObject.Find("AuxCanvas");
        auxiliaryLog = GameObject.Find("AuxLogText1");
        auxiliaryLog2 = GameObject.Find("AuxLogText2");
        activeLogMessageStrips[0] = GameObject.Find("LogText1");
        activeLogMessageStrips[1] = GameObject.Find("LogText2");
        activeLogMessageStrips[2] = GameObject.Find("LogText3");
        activeLogMessageStrips[3] = GameObject.Find("LogText4");
        activeLogMessageStrips[4] = GameObject.Find("LogText5");

        auxCanvas.SetActive(false);

        logMessages.Clear();
        for (int v = 0; v < 5; v++)
        {
            //logMessage("hello " + v);
        }
        foreach(string s in activeLogMessages)
        {
            Debug.Log(s);
        }
       
        foreach (string s in activeLogMessages)
        {
            Debug.Log(s);
        }

        //logMessage("The yoghurt blurps with horrendous vim.");
   
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            scrollLog.transform.Rotate(-Vector3.right, 400 * Time.deltaTime, Space.World);
            //logMessage("test message " + counter);
            counter += 1;
        }

    }

    // A method that takes new messages and pops it into the array of messages, rotates the roll, removes old messages and adds the new one to the top
    public void logMessage(string s)
    {
        string s1 = null;
        string s2 = null;
        
        // keep complete log
        logMessages.Add(s);
        
        // Establish length of log
        // 
        int maxLineLength = 39;
        bool multiLine = false;
        if(s.Length > maxLineLength)
        {
            // need to split across multiple lines
            multiLine = true;
            if (multiLine)
            {

                for (int i = 0; i < s.Length; i++)
                {
                    if(i % maxLineLength == 0)
                    {
                        s1 = s.Substring(0, maxLineLength);
                        s2 = s.Substring(maxLineLength, s.Length -maxLineLength);
                   
                    }
                }

            }
        }

        // rotate log
        scrollLog.transform.Rotate(-Vector3.right, 250 * Time.deltaTime, Space.World);
        
        // update active messages 
        if(multiLine)
        {
            activeLogMessages[4] = activeLogMessages[2];
            activeLogMessages[3] = activeLogMessages[1];
            activeLogMessages[2] = activeLogMessages[0];

            activeLogMessages[0] = s1;
            activeLogMessages[1] = s2;
        }
        else
        {
            for (int i = 4; i > 0; i--)
            {
                activeLogMessages[i] = activeLogMessages[i - 1];
            }

            activeLogMessages[0] = s;

        }

        // Actually update the texts
        for (int t = 0; t < activeLogMessages.Length; t++)
        {
            activeLogMessageStrips[t].GetComponent<Text>().text = activeLogMessages[t];
        }

        
        // Update auxiliary log
       // if (zoomedIn)
       // {


            if (s1 != null)
            {
                // Remember that Text accessing requires UnityEngine.UI
                auxiliaryLog.GetComponent<Text>().text = s1;
                auxiliaryLog2.GetComponent<Text>().text = s2;
            }
            else
            {
                auxiliaryLog2.GetComponent<Text>().text = s;
                auxiliaryLog.GetComponent<Text>().text = "";    
            }
        //}

    }
}
