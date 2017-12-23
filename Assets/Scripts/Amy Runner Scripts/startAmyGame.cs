using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startAmyGame : MonoBehaviour {

    /*This script was just added so that I wouldn't have to rememeber to enable the Tutorial panel and disable
     * Amy's Sprite Renderer every time I fininsh working on this scene
     */

    public SpriteRenderer amySR;
    public GameObject tutorialPanel, basicPanel;

	void OnEnable () {
        amySR.enabled = false;
        basicPanel.SetActive(false);
        tutorialPanel.SetActive(true);

        Time.timeScale = 0;
	}
	
}
