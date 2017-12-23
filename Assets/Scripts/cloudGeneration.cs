using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// This class actually managaes the procedural generation of all platforms across the levels, 
/// not just the cloud level
/// </summary>
public class cloudGeneration : MonoBehaviour {

    //These are variables for the dimentional portals
    public GameObject portal;
    public GameObject bossPortal;
    
    //These variables are here so that the generation will stop once the player is far away.
    public Transform player;
    float maxDistance = 75;

    float minWaitTime = 0;

    //The limits of the x coordinates of the platforms
    public Transform maxY;
    public Transform minY;

    //Max and min distance between the randomly generated clouds
    public float distanceTop;
    public float distanceBottom;

    public List<GameObject> cloudOptions = new List<GameObject>();

    //Coin variables
    public List<GameObject> coinPatterns = new List<GameObject>(); //Filled in through the Unity inspector
    float jumpHeight; //The maximum height of a standard jump of the player



    // Use this for initialization
    void Start () {

        jumpHeight = Mathf.Pow(player.GetComponent<characterMovement>().jumpSpeed, 2) / (2 * Mathf.Abs(Physics2D.gravity.y));

    }
	

	void LateUpdate () {
        //Making clouds if player isn't too far away
        if (Vector3.Distance(transform.position, player.position) < maxDistance) generateNextCloud(cloudOptions);  
	}

    void generateNextCloud(List<GameObject> list)
    {
        if (Time.time > minWaitTime)
        {
            //Calculating the cloud to be made and some values
            float distanceInbetween = Random.Range(distanceBottom, distanceTop);
            int indicator = Random.Range(0, cloudOptions.Count);
            float yPosition = Random.Range(minY.position.y, maxY.position.y);

            //Making the cloud
            GameObject cloudMade = Instantiate(cloudOptions[indicator], new Vector3(transform.position.x + Random.Range(0.1f, 1f), yPosition, transform.position.z), Quaternion.Euler(0,0,0));
            Transform edge = cloudMade.transform.Find("edge").transform;

            //Shifting the cloud to the right in reference to its boundaries
            cloudMade.transform.position = new Vector3(edge.position.x, cloudMade.transform.position.y, cloudMade.transform.position.z);

            //Moving the generator to the cloud's edge
            transform.position = new Vector3(edge.position.x , transform.position.y, transform.position.z);

            //Moving the cloud generator
            transform.position = new Vector3(transform.position.x + distanceInbetween, transform.position.y, transform.position.z);

            doPortalCalculations(edge);

            //Determining if coins will be placed  
            if (Random.Range(0, 2) == 1) generateCoinPattern(cloudMade);

            //Adding wait time
            minWaitTime = Time.time + 0.3f;
        }
    }


    void makePortal(GameObject portal, Vector3 position)
    {
        GameObject realPortal = Instantiate(portal,
                                            position,
                                            Quaternion.Euler(0, 0, 0));

        //Giving the portal it's player/target that it is permitted suck in.
        realPortal.transform.Find("Dimensional Portal").GetComponent<shrinkPlayer>().player = player.gameObject;
        realPortal.transform.Find("Dimensional Portal").GetComponent<shrinkPlayer>().getReady();
    }

    void doPortalCalculations(Transform edge)
    {
            //If the player can't go to the boss, there's a 10% chance of a portal spawning
            if (!globalDataPreserver.Instance.canBoss && !globalDataPreserver.Instance.canBossPermanent)
            {
                if (Random.Range(0, 10) == 1)
                {
                    makePortal(portal, new Vector3(Random.Range(edge.position.x, transform.position.x), minY.position.y, transform.position.z));
                }
            }
            else if (globalDataPreserver.Instance.canBoss)
            {
                //If the player can go to the boss, there is a 13.3% change of a portal spawning
                if (Random.Range(0, 15) < 2)
                {
                    //75% of those portals will be boss portals
                    if (Random.Range(0, 3) != 1)
                    {
                        makePortal(bossPortal, new Vector3(Random.Range(edge.position.x, transform.position.x), minY.position.y, transform.position.z));
                    }
                    else //The rest will be regular portals
                    {
                        makePortal(portal, new Vector3(Random.Range(edge.position.x, transform.position.x), minY.position.y, transform.position.z));
                    }
                }
            }
            else if (globalDataPreserver.Instance.canBossPermanent) //If the player has been to the boss several times and can go again...
            {
                //There's a ~8.3% change of a portal spawning
                if (Random.Range(0, 12) == 1)
                {
                    //50% will be boss portals
                    if (Random.Range(0,2) == 1)
                    {
                        makePortal(portal, new Vector3(Random.Range(edge.position.x, transform.position.x), minY.position.y, transform.position.z));
                    }else
                    {
                        makePortal(bossPortal, new Vector3(Random.Range(edge.position.x, transform.position.x), minY.position.y, transform.position.z));
                    }
                }
            }

    }

    void generateCoinPattern(GameObject cloud)
    {
        BoxCollider2D cloudCollider = cloud.GetComponent<BoxCollider2D>();
        float cloudTop = cloudCollider.bounds.max.y;
        float yposition = Random.Range(cloudTop, cloudTop + jumpHeight);
        float xposition = Random.Range(cloudCollider.bounds.min.x, transform.position.x);
        Instantiate(coinPatterns[Random.Range(0, coinPatterns.Count)], new Vector3(xposition, yposition, 0), Quaternion.Euler(0, 0, 0));

    }
}
