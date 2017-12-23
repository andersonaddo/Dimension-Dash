using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scoreSynchronization : MonoBehaviour {

    Text score;

	// Use this for initialization
	void Start () {
        score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        score.text = globalDataPreserver.Instance.playerScore.ToString();
	}
}
