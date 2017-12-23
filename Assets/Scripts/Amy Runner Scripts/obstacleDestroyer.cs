using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleDestroyer : MonoBehaviour {

    public AudioClip rockDestructionClip;
    AudioSource parentAudioSource;

	// Use this for initialization
	void Start () {
        parentAudioSource = GetComponentInParent<AudioSource>();
	}
	
	void OnTriggerEnter2D (Collider2D other) {
        if (other.tag.Equals("Dangerous"))
        {
            parentAudioSource.PlayOneShot(rockDestructionClip);
            other.GetComponent<rockDestruction>().destroy();
            globalDataPreserver.Instance.playerScore += 100;
        }
	}
}
