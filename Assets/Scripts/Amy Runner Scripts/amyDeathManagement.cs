using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amyDeathManagement : MonoBehaviour {

    public GameObject laneGenerator, 
                      deathPanel, 
                      basicPanel, 
                      obstacleParent,
                      background1, 
                      background2;

    //Camera movement variables
    public Transform mainCamera;
    bool shiftCamera;
    Vector3 cameraDestination;
    public float speed;

	// Use this for initialization
	void Start () {
        //The camera will slowly pan here before the death screen appears
        cameraDestination = new Vector3(mainCamera.transform.position.x + 5, mainCamera.transform.position.y, mainCamera.transform.position.z);
	}

    void Update()
    {
        if (shiftCamera) mainCamera.position = Vector3.Lerp(mainCamera.position, cameraDestination, speed * Time.deltaTime);
    }
	
	//Called externally by animator when death animation finishes
	void finishDeath () {
        laneGenerator.GetComponent<laneObstacleGeneration>().stopGeneration(); //Stopping lane generation

        //Stopping already generated obstacles and stopping the backgrounds.
        obstacleParent.BroadcastMessage("stop");
        background1.BroadcastMessage("stop");
        background2.BroadcastMessage("stop");

        StartCoroutine("endSequence");

    }

    IEnumerator endSequence()
    {
        basicPanel.SetActive(false);

        //Shifting the camera
        shiftCamera = true;
        yield return new WaitUntil(() => Vector2.Distance(cameraDestination, mainCamera.transform.position) < 0.1);
        shiftCamera = false;

        yield return new WaitForSeconds(1);

        //Bringing the death panel
        Time.timeScale = 0;
        deathPanel.SetActive(true);   
    }
}
