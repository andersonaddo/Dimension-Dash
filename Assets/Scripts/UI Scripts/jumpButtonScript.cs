using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jumpButtonScript : MonoBehaviour {

    public GameObject player;
    characterMovement myCR;


	// Use this for initialization
	void Start () {
        myCR = player.GetComponent<characterMovement>();

        //Setting transparency to the player preferences' transparency.
        Image material = GetComponent<Image>();
        material.color = new Color(material.color.r, material.color.g, material.color.b, globalDataPreserver.Instance.getButtonOpacity()/255);
        Text buttonText = GetComponentInChildren<Text>();
        buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, material.color.a);
    }


    public void jumpCharacter()
    {
        myCR.jump();
    }
}
