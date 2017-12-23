using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class shrinkPlayer : MonoBehaviour {

    //Player variables
    public GameObject player;
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public characterMovement playerMovement;
    
    //Shrinking variables
    public static float targetScale = 0.1f;
    public float shrinkSpeed = 0.1f;
    float acceptDiff = targetScale + 0.2f;

    public LayerMask playerLayer;
    float shrinkRadius = 1.5f;

    //Variables added the portal can behave differently if it's a boss portal
    public bool isBossPortal;
    bool canSwitch;



    // Use this for setting the portal up.
    public void getReady () {
        rb = player.GetComponent<Rigidbody2D>();
        playerMovement  = player.GetComponent<characterMovement>();
	}

    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {

            if (Physics2D.OverlapCircle(transform.position, shrinkRadius, playerLayer))
            {
                shrink();
            canSwitch = true;
            }

            //If player is small enough, increase the score and switch scenes
            if (player.transform.localScale.y < acceptDiff && canSwitch)
            {
                globalDataPreserver.Instance.playerScore += 80;
                switchScenes();  
            }    
	
	}



    void switchScenes()
    {
        if (isBossPortal)
        {
            globalDataPreserver.Instance.canBoss = false; //Preventing the player from entering the boss' realm again unitll the score is high enough
            prepareForBoss();
            SceneManager.LoadScene(4);
        }else
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int index;
            List<int> availableDimensions = globalDataPreserver.Instance.getAvailableDimensions();

            //Loading any level apart from the current one
            while (true)
            {
                index = availableDimensions[Random.Range(0, availableDimensions.Count)];
                if (index != currentIndex)
                {
                    break;
                }
            }

            //Saving the player's current score incase the player is going to face the boss
            globalDataPreserver.Instance.previousScore = (int)globalDataPreserver.Instance.playerScore;

            //Loading the scene
            SceneManager.LoadScene(index);
        }
       
    }


    void shrink()
    {
        //Stop all possible movement
        playerMovement.baseSpeed = 0f;
        playerMovement.currentSpeed = 0f;
        playerMovement.isGrounded = false;
        playerMovement.hasDoubleJumped = true;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0,0);

        //Shrinking
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime * shrinkSpeed);
    }

    //called when the player is being taken to the boss level to prepare the global data variables
    void prepareForBoss()
    {
        globalDataPreserver.Instance.previousScore = (int)globalDataPreserver.Instance.playerScore;
        if (globalDataPreserver.Instance.encounters >= 4) globalDataPreserver.Instance.encounters++;
    }
}
