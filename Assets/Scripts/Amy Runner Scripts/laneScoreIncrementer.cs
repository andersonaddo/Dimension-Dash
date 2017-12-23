using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneScoreIncrementer : MonoBehaviour {

    public int increment;
	
    //Increments the score whenever the player passes a lane
	void OnTriggerExit2D () {
        globalDataPreserver.Instance.playerScore += increment;
	}
}
