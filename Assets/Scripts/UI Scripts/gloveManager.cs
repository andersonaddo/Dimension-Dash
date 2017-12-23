using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloveManager : MonoBehaviour {

	// Diabling the pointer if the game has been player before
	void Start () {
        internalMemoryManager myIMM = new internalMemoryManager();
        gameObject.SetActive(myIMM.loadHiScore() == 0);
	}
	
}
