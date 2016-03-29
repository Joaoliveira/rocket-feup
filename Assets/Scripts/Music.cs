using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

    public AudioClip audioclip1;
    public AudioClip audioclip2;
    public AudioClip audioclip3;
    public AudioClip audioclip4;

    AudioSource audio;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
        int rand = Random.Range(1, 4);
        switch(rand) {
            case 1:
                audio.clip = audioclip1;
                break;
            case 2:
                audio.clip = audioclip2;
                break;
            case 3:
                audio.clip = audioclip3;
                break;
            case 4:
                audio.clip = audioclip4;
                break;
        }
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
