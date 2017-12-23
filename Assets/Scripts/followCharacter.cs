using UnityEngine;
using System.Collections;

public class followCharacter : MonoBehaviour {

    //UI variables
    public GameObject basicPanel, deathPanel;

    public Transform character;
    public float smoothing;
    public float minY; 
    public float yDiff;

    bool isFollowing = true; //An extra measure to ensure that the death coroutine is called only once. Remove it and see what happens

    [HideInInspector] public bool isDead;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
        if (transform.position.y > minY)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(character.position.x + 7, character.position.y + yDiff, transform.position.z), smoothing * Time.deltaTime);
            }
            else
            {
                transform.position = transform.position;        
        }
        
        
        //isDead will be mde externally when the character dies.
        if (isDead && isFollowing)
        {
            StartCoroutine("makeDeathMenu");
        }
    }

    IEnumerator makeDeathMenu()
    {
        basicPanel.SetActive(false);
        isDead = false;
        isFollowing = false;
        character.gameObject.SetActive(false);

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);

        //Hiding the pause button and opening the death panel
        deathPanel.SetActive(true);
    }

}