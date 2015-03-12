using UnityEngine;
using System.Collections;

public class Pulsate : MonoBehaviour {

    public float minIntensity = 0.4f;
    public float maxIntensity = 1.6f;
    private bool goingUp = true;
    public float speed = 0.01f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(goingUp && this.transform.light.intensity < maxIntensity)
        {
            this.transform.light.intensity += Time.deltaTime * speed;
        } else if (this.transform.light.intensity >= maxIntensity)
        {
            goingUp = false;
            this.transform.light.intensity -= Time.deltaTime * speed;
        } else if (!goingUp)
        {
            this.transform.light.intensity -= Time.deltaTime * speed;
        } 
        
        if (this.transform.light.intensity <= minIntensity)
        {
            goingUp = true;
            this.transform.light.intensity += Time.deltaTime * speed;
        }

	}
}
