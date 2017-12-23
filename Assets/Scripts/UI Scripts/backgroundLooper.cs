using UnityEngine;
using System.Collections;

public class backgroundLooper : MonoBehaviour {

    public Transform alternative;
    float size;

	// Use this for initialization
	void Start () {
        size = GetComponentInParent<Renderer>().bounds.size.x;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

   void OnTriggerExit2D (Collider2D other)
    {
        //Checking if the object is indeed the player
        if (other.gameObject.tag == "Player")
        {
            //Move the background behind you to infront of you
            alternative.position = new Vector3(transform.position.x + size, transform.position.y, transform.position.z);
        }
    }
}
