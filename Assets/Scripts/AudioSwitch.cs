using UnityEngine;
using System.Collections;

public class AudioSwitch : MonoBehaviour {

    public AudioClip clip1;
    public AudioClip clip2;

	// Use this for initialization
	void Start () {
        if(Random.Range(0,10)<5)
        {
            audio.clip = clip2;

        }
        else
        {
            audio.clip = clip1;
        }
        audio.Play();
       // AudioSource.PlayOneShot(AudioClip[0]);
	
	}

    public void randomiseMusic()
    {

        if (Random.Range(0, 10) < 5)
        {
            audio.clip = clip2;

        }
        else
        {
            audio.clip = clip1;
        }
        audio.Play();


    }
	/*void PlaySound()
    {
        //Assign random sound from variable
        audio.clip = otherClip[Random.Range(0,otherClip.length)];
 
        audio.Play();
 
        // Wait for the audio to have finished
        yield WaitForSeconds (audio.clip.length);
 
        //Now you should re-loop this function Like
        PlaySound();
    */
	// Update is called once per frame
	void Update () {
	
	}
}
