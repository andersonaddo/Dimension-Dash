using UnityEngine;
using System.Collections;

public class suckPlayerIn : MonoBehaviour {

    public float force;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D (Collider2D other)
    {
        //Checking if the gameObject is the player
        if (other.gameObject.tag == "Player")
        {
            //Suck the player in
            GameObject PullOBJ = other.gameObject;

            PullOBJ.transform.position = Vector3.MoveTowards
                (PullOBJ.transform.position,
                 transform.position,
                 force * Time.deltaTime);
        }
    }
}
