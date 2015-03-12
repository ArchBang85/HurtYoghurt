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
    public GameObject auxiliaryLog; // What to display as an overlay when zoomed in 

    // Use this for initialization
    void Start()
    {
        scrollLog = GameObject.Find("ScrollLog");
        auxiliaryLog = GameObject.Find("AuxLogText1");

        logMessages.Clear();
        for (int v = 0; v < 5; v++)
        {
            logMessage("hello " + v);
        }
        foreach(string s in activeLogMessages)
        {
            Debug.Log(s);
        }
        logMessage("Hello there, the yoghurt burps ");
        foreach (string s in activeLogMessages)
        {
            Debug.Log(s);
        }
   
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            scrollLog.transform.Rotate(-Vector3.right, 250 * Time.deltaTime, Space.World);
        }

    }

    // A method that takes new messages and pops it into the array of messages, rotates the roll, removes old messages and adds the new one to the top
    void logMessage(string s)
    {
        string s1 = null;
        string s2 = null;
        
        // keep complete log
        logMessages.Add(s);
        
        // Establish length of log
        // 
        int maxLineLength = 26;
        bool multiLine = false;
        if(s.Length > maxLineLength)
        {
            // need to split across multiple lines
            multiLine = true;
            for (int i = 0; i < s.Length; i++)
            {
                if(i % maxLineLength == 0)
                {
                    s1 = s.Substring(0, maxLineLength);
                    s2 = s.Substring(maxLineLength + 1, s.Length);
                }
            }
        }

        // rotate log
        scrollLog.transform.Rotate(-Vector3.right, 250 * Time.deltaTime, Space.World);
        
        // update active messages 

        for (int i = 4; i > 0; i--)
        {
            activeLogMessages[i] = activeLogMessages[i - 1];
        }

        activeLogMessages[0] = s;

        // Update auxiliary log
        if (s1 != null)
        {
            auxiliaryLog.GetComponent<Text>().text = s1;
        }
        else
        {
            auxiliaryLog.GetComponent<Text>().text = s;
        }
            //optionChosen.GetComponent<Text>()
    }
}
