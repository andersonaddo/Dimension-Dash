using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleportAmyOut : MonoBehaviour {

    //Player variables
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Rigidbody2D rb;
    

    //Shrinking variables
    public static float targetScale = 0.1f;
    public float shrinkSpeed = 0.1f;
    float acceptDiff = targetScale + 0.5f;

    public LayerMask playerLayer;
    float shrinkRadius = 1.5f;

    public int addedScore;
    bool canSwitch;



    // Use this for setting the portal up.
    public void getReady()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Physics2D.OverlapCircle(transform.position, shrinkRadius, playerLayer))
        {
            shrink();
            canSwitch = true;
        }

        //If player is small enough, increase the score and switch scenes
        if (canSwitch)
        {
            if (player.transform.localScale.x < acceptDiff)
            {
                globalDataPreserver.Instance.playerScore += addedScore;
                switchScenes(); 
            }

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
        //Shrinking
        player.transform.localScale = Vector3.Lerp(player.transform.localScale, new Vector3(targetScale, targetScale, targetScale), Time.deltaTime * shrinkSpeed);
    }
}
