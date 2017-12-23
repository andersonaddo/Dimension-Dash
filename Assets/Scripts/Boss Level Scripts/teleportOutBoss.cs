using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class teleportOutBoss : MonoBehaviour {

    //Player variables
    public GameObject player;
    Rigidbody2D rb;
    stationaryCharacter mySC;
    Animator myAnim;
    
    //Shrinking variables
    public static float targetScale = 0.1f;
    public float shrinkSpeed = 0.1f;
    float acceptDiff = targetScale + 0.2f;

    public LayerMask playerLayer;
    float shrinkRadius = 1.5f;



    // Use this for setting the portal up.
    public void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        mySC = player.GetComponent<stationaryCharacter>();
        myAnim = player.GetComponent<Animator>();
    }

    

    // Update is called once per frame
    void Update()
    {

        if (Physics2D.OverlapCircle(transform.position, shrinkRadius, playerLayer))
        {
            shrink();
        }

        //If player is small enough, increase the score and switch scenes
        if (player.transform.localScale.x < acceptDiff)
        {
            globalDataPreserver.Instance.playerScore += getReward();
            switchScenes();
        }


    }



    void switchScenes()
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

        //Loading the scene
        SceneManager.LoadScene(index);
    }


    void shrink()
    {
        //Stop all possible movement
        mySC.isGrounded = false;
        mySC.hasDoubleJumped = true;
        mySC.dashSpeed = 0;
        mySC.isDashing = true;

        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, 0);

        myAnim.SetBool("isGrounded", true);

        //Shrinking
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime * shrinkSpeed);
    }


    //Calculates the reward score the player gets accoring to how hard the boss was
    int getReward()
    {
        switch (globalDataPreserver.Instance.encounters)
        {
            case 1:
                return 1700;
            case 2:
                return 3500;
            case 3:
                return 4600;
            case 4:
                return 5700;
            case 5:
                return 7000;
            default:
                return 7000;
        }
    }
}
