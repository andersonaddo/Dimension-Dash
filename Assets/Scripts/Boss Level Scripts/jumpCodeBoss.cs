using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The code foe the jumo button of the scene
/// </summary>
public class jumpCodeBoss : MonoBehaviour {

	public GameObject player;
    stationaryCharacter mySR;

    bool isEnabled;

	// Use this for initialization
	void Start () {
        isEnabled = true;
        mySR = player.GetComponent<stationaryCharacter>();

        //Setting transparency to the player preferences' transparency.
        Image material = GetComponent<Image>();
        material.color = new Color(material.color.r, material.color.g, material.color.b, globalDataPreserver.Instance.getButtonOpacity() / 255);
        Text buttonText = GetComponentInChildren<Text>();
        buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, material.color.a);

    }

    public void setEnabled(bool state)
    {
        isEnabled = state;
    }

    public void jumpCharacter()
    {
        if (isEnabled)
        {
            mySR.jump();
            isEnabled = false;
        }
    }
}
