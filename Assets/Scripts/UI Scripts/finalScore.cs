using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Score UI on the Death screen
/// </summary>
public class finalScore : MonoBehaviour {

    public bool isBossLvl;
    Text text;

    void Start()
    {
        
    }

	void OnEnable ()
    {
        text = gameObject.GetComponent<Text>();

        //Determining what exactly is going to be showed and compared to the current high score
        int endgameScore;
        if (isBossLvl)
        {
            //If the player dies in a boss level, their final score is the score they had before the encountered the boss
            endgameScore = globalDataPreserver.Instance.previousScore;
        }else
        {
            endgameScore = (int)globalDataPreserver.Instance.playerScore;
        }

        //Showing the score decided upon
        text.text = "" + endgameScore;

        //Determing the current high score
        globalDataPreserver.Instance.calculateHighScore(endgameScore);
    }
	
}
