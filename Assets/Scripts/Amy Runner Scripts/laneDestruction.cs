using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneDestruction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D (Collider2D other) {
        if(!other.tag.Equals("Player"))Destroy(other.gameObject);
	}
}
