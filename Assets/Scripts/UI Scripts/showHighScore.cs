using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showHighScore : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
        GetComponent<Text>().text = "" + globalDataPreserver.Instance.currentHighScore;
	}
	
}
