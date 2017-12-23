using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class amyMovement : MonoBehaviour {

    Animator myAnim;
    Transform parentTransform; //Since this script is attatched to a child oblect, this refers to the character's parent
    amyPickupManager pickUpScript;

    //Lane variables
    public float switchingSpeed;
    public List<Transform> lanes;
    int noOfLanes;
    int currentLane;
    Vector2 targetPoint; //The location of the lane the character is meant to be on


    //Hammer variables
    public bool isHammering;
    public float hammerRate; //How many seconds must pass before a hammering session can be initiated again
    public float hammerDuration; //How long hammering sessions last
    bool canHammer = true;
    public GameObject hammerCollider;


    public int hammerCount; //How many hammer power-ups Amy has

    //Touch Variables
    private Vector3 firstPos;   //First touch position
    private Vector3 lastPos;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    bool isTouchValid = true;



    // Use this for initialization
    void Start () {
        myAnim = GetComponent<Animator>();
        pickUpScript = GetComponent<amyPickupManager>();
        noOfLanes = lanes.Count;
        parentTransform = transform.parent.transform;

        //Putting the character on their initial lane (the middle lane)
        currentLane = Mathf.RoundToInt(noOfLanes / 2);
        targetPoint = new Vector2 (parentTransform.position.x, lanes[currentLane-1].position.y);
        parentTransform.position = targetPoint;

        dragDistance = Screen.height * 9 / 100; //dragDistance is 9% height of the screen
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        doKeyboardControls();
#else
        doTouchCalculations(); 
#endif

        parentTransform.position = Vector2.MoveTowards(parentTransform.position, targetPoint, switchingSpeed);

    }



    //Called when the player starts a hamemring session
    public void useHammer()
    {
        if (pickUpScript.hammerCount > 0 && !pickUpScript.isDead && canHammer) //If the hammer count is greater than zero, then start the session
        {
            isHammering = true;
            StartCoroutine("hammerSession");
            pickUpScript.reduceHammerCount();
        }
    }

    IEnumerator hammerSession()
    {
        canHammer = false; //Can't hammer unitl this session is finished
        myAnim.SetBool("isUsingHammer", true);
        hammerCollider.SetActive(true);
        yield return new WaitForSeconds(hammerDuration); //Waiting for the hammer session to end
        isHammering = false; //Stopping the hammering session
        myAnim.SetBool("isUsingHammer", false);
        hammerCollider.SetActive(false);
        yield return new WaitForSeconds(hammerRate); //Waiting until the player can hammer again
        canHammer = true;
    }

    void swtichLanes(int degree)
    {
        if ((currentLane + degree) <= lanes.Count && (currentLane + degree) >= 1 && !pickUpScript.isDead)
        {
            currentLane += degree;
            targetPoint = new Vector2(parentTransform.position.x, lanes[currentLane - 1].position.y);
        }
    }

    //Called externally by animator when death animation ends
    public void enableHammerOnDeath()
    {
      hammerCollider.SetActive(true);
    }



    void doKeyboardControls()
    {
        if (Input.GetButtonDown("Vertical")) swtichLanes(-(int)Input.GetAxisRaw("Vertical"));
        if (Input.GetButtonDown("Jump")) useHammer();
    }




    void doTouchCalculations()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                firstPos = touch.position;
                lastPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lastPos = touch.position;

                //Check if drag distance is greater than 20% of the screen height. If it is, then register a drag
                if ((Mathf.Abs(lastPos.x - firstPos.x) > dragDistance || Mathf.Abs(lastPos.y - firstPos.y) > dragDistance) && isTouchValid)
                {
                    isTouchValid = false;

                    if (Mathf.Abs(lastPos.x - firstPos.x) > Mathf.Abs(lastPos.y - firstPos.y))
                    {
                        if (lastPos.x > firstPos.x) useHammer(); //Right swipe
                    }
                    else
                    {
                        if (lastPos.y < firstPos.y) swtichLanes(1); //Downwards Swipe
                        else swtichLanes(-1);//Upwards Swipe
                    }

                }
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                isTouchValid = true;
            }
        }

    }

}
