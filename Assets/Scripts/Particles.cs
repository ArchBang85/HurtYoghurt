using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour {
    public float selfDestructTimer = 1.0f;
	// Use this for initialization
    void Start()
    {
        //Change Foreground to the layer you want it to display on 
        //You could prob. make a public variable for this
        particleSystem.renderer.sortingLayerName = "Effects";
        Destroy(this.gameObject, selfDestructTimer);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
