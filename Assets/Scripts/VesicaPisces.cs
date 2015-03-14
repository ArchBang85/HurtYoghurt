using UnityEngine;
using System.Collections;

public class VesicaPisces : MonoBehaviour {

    // Create four overlapping spheres and one outside vestibule
    public GameObject pisces;
    public GameObject smallPisces;

    public int stages = 4;
    public int copies = 1;
    private Transform piscesParts;

    void start()
    {


    DontDestroyOnLoad(gameObject);
    }
     
    // Use this for initialization
	public void setupPisces () {
        if (piscesParts == null)
        {
            if (GameObject.Find("piscesHolder") == null)
            {
                piscesParts = new GameObject("piscesHolder").transform;
            }
            else
            {
                piscesParts = GameObject.Find("piscesHolder").transform;
            }
        }
        
        for (int y = 0; y < copies; y++)
        {
            for (int i = 0; i < stages; i++)
            {
                GameObject instance = Instantiate(pisces, new Vector3(transform.position.x + i * pisces.transform.localScale.x / 2, transform.position.y + y * 10, transform.position.z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(piscesParts);
            }
            // choose one intersection
            // For each 5th element, add a new side bulb
            int bulbs = 0;
            int s = stages;
            while (s > 0)
            {
                bulbs += 1;
                s -= 4;
            }

            for (int b = 0; b < bulbs; b++)
            {

                int horisontal = Random.Range(0, stages / 2);
                int vertical = Random.Range(0, 2);
                if (vertical == 0)
                {
                    vertical = -1;
                }
                GameObject instance = Instantiate(pisces, new Vector3(transform.position.x + pisces.transform.localScale.x / 2 + horisontal * pisces.transform.localScale.x / 2, transform.position.y + vertical * pisces.transform.localScale.y / 2 + y * 10, transform.position.z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(piscesParts);
            }

            // optional one small piscesSphere
            if(Random.Range(0, 1)<1)
            {
                int horisontal = Random.Range(0, stages / 2 + 1);
                int vertical = Random.Range(0, 2);
                if (vertical == 0)
                {
                    vertical = -1;
                }
                GameObject instance = Instantiate(smallPisces, new Vector3(transform.position.x + pisces.transform.localScale.x / 4 + horisontal * pisces.transform.localScale.x / 2, transform.position.y + vertical * pisces.transform.localScale.y / 2 + y * 10, transform.position.z), Quaternion.identity) as GameObject;
                instance.transform.SetParent(piscesParts);
            }


        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
