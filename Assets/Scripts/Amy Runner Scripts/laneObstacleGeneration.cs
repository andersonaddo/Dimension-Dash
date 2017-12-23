using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneObstacleGeneration : MonoBehaviour {

    //The obstacles and pick-ups
    public GameObject coin, hammer;


    //The endgame portal variables
    int noOfLanesGenerated = 0;
    public GameObject portal;
    public int maxNoOfLanes;

    public List<GameObject> stones;
    public List<Transform> lanes;
    public GameObject scoreIncrementer;

    //This is the parent gameobject of all the instanciated obstacles.
    //They are all put here so that they can be affected by a broadcast and all stop when the player dies
    public Transform obstaclesParent;

    //Generation variables
    int noOfFilled;
    bool canGenerate = true;
    //Difficulty variables
    public float speedIncrement;
    public float waitTime;
    public float minWaitTime;


    // Use this for initialization
    void Start () {
        StartCoroutine("constantGeneration");
	}

    IEnumerator constantGeneration()
    {
        while (true)
        {
            if (!canGenerate) break;

            //Checking if endgame portal should be generated yet
            if (noOfLanesGenerated == maxNoOfLanes)
            {
                yield return new WaitForSeconds(1.5f);
                if (!canGenerate) break;
                GameObject generatedPortal = Instantiate(portal, transform.position, Quaternion.Euler(0,0,90));
                generatedPortal.transform.parent = obstaclesParent;

                break;
            }

            //Making a row of obstacles
            for (int i = 0; i < lanes.Count; i++)
            {
                if (i == lanes.Count - 1 && noOfFilled == i) break; //Stops the generation if all the lanes have been filled and the last lane is being worked on

                Transform currentLane = lanes[i];
                bool toBeFilled = (Random.Range(1, 3) == 2);

                //This if statement automatically becomes true if no lanes have been blocked and the last lane is being processed
                if (toBeFilled || (noOfFilled == 0 && i == lanes.Count-1)) 
                {
                    //Putting a stone in the lane
                    GameObject stone = Instantiate(stones[Random.Range(0, stones.Count)], new Vector2(transform.position.x, currentLane.position.y), Quaternion.Euler(0, 0, 0));

                    stone.transform.parent = obstaclesParent; //Putting it in it's intended parent (check declaration)

                    noOfFilled++;
                }else
                {
                    getPickUp(currentLane);
                }
            }

            //Placing down a score incrementer (a trigger collider)
            GameObject incrementer = Instantiate(scoreIncrementer, transform.position, transform.rotation);
            incrementer.transform.parent = obstaclesParent; //Putting it in it's intended parent (check declaration)

            //After all the lanes have been worked on, wait a while
            noOfFilled = 0;
            yield return new WaitForSeconds(waitTime);

            noOfLanesGenerated++;

            //Making the game a little harder with each lane
            if (waitTime > minWaitTime) waitTime -= speedIncrement;
        }
    }

    void getPickUp(Transform lane)
    {
        GameObject producedObject;

        //Making coin in a 50% chance
        if (Random.Range(1, 3) == 2)
        {
            producedObject = Instantiate(coin, new Vector2(transform.position.x, lane.position.y), Quaternion.Euler(0, 0, 0));
            producedObject.transform.parent = obstaclesParent;
            return;
        }

        //Making a hammer in a 10% chance (0.4 * 0.25)
        if (Random.Range(1,5) == 1)
        {
            producedObject = Instantiate(hammer, new Vector2(transform.position.x, lane.position.y), Quaternion.Euler(0, 0, 0));
            producedObject.transform.parent = obstaclesParent;
            return;
        }
    }

    //Called when player dies
    public void stopGeneration()
    {
        canGenerate = false;
    }
}
