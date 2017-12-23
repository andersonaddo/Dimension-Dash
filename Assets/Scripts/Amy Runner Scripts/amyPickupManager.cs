using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class amyPickupManager : MonoBehaviour {

    Animator myAnim;

    public Text hammerCountText;
    public int hammerCount = 0;

    [HideInInspector]public bool isDead;

    public GameObject hammerPlusOne;
    public Transform obstacleParent;

    public AudioClip hammerCollectSound;
    AudioSource myAudioSource;

	// Use this for initialization
	void Start () {
        myAnim = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        //If the obstacle is a stone
        if (other.tag.Equals("Dangerous"))
        {
            startDeath();
        }else if (other.tag.Equals("Hammer ")) //That means it's a hammer then...
        {
            hammerCount++;
            hammerCountText.text = "x" + (hammerCount);

            GameObject hammerNotice = Instantiate(hammerPlusOne, other.transform.position, Quaternion.Euler(0, 0, 0)); //Making the +1 notice
            hammerNotice.transform.SetParent(obstacleParent, false);

            myAudioSource.PlayOneShot(hammerCollectSound);
            Destroy(other.gameObject);
        }
    }

    void startDeath()
    {
        myAnim.SetBool("isFalling", true);
        isDead = true;
    }

    public void reduceHammerCount()
    {
        hammerCount--;
        hammerCountText.text = "x" + (hammerCount); //Updating the UI
    }

}
