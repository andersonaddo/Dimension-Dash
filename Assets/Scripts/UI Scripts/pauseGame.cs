using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseGame : MonoBehaviour {

	public void pause()
    {
        Time.timeScale = 0;
    }
}