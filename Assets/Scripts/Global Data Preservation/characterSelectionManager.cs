using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class characterSelectionManager : MonoBehaviour {

    int index;

    //Character options
    public List<GameObject> charactersAvailable = new List<GameObject>();

    //Variables for scene objects
    public GameObject bonusText;
    public GameObject gameCamera;
    public GameObject jumpButton;
    public GameObject cloudGenerator;

    // Use this for initialization
    void Awake() {

        index = globalDataPreserver.Instance.characterChoices[SceneManager.GetActiveScene().buildIndex].selectedCharacter;

        //Instantiating the player
        GameObject player = Instantiate(charactersAvailable[index], transform.position, Quaternion.Euler(0, 0, 0));

        //Setting the player as the  target for the camera
        gameCamera.GetComponent<followCharacter>().character = player.transform;

        //Setting the character's skipper UI
        player.GetComponentInChildren<platformSkipping>().bonusGUI = bonusText;

        //The player also has a reference to the game camera, so I'm also setting it up
        player.GetComponent<characterMovement>().gameCamera = this.gameCamera.GetComponent<Camera>();

        //Setting up the player reference for the jump button too 
        jumpButton.GetComponent<jumpButtonScript>().player = player;

        //Lastly, setting the player reference for the cloudGenerator
        cloudGenerator.GetComponent<cloudGeneration>().player = player.transform;
	}
}
