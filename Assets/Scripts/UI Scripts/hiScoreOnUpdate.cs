using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class hiScoreOnUpdate : MonoBehaviour {

    void Update()
    {
        GetComponent<Text>().text = "" + globalDataPreserver.Instance.currentHighScore;
    }
}
