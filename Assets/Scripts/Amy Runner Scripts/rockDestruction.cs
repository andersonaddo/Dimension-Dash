using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockDestruction : MonoBehaviour {

    public GameObject crumbleExplosion;

    // Use this for initialization
    void Start () {
		
	}
	
    //Called externally by Amy's Obstacle Destroyer
	public void destroy()
    {
        GameObject crumbles = Instantiate(crumbleExplosion, transform.position, Quaternion.Euler(0, 0, 0));
        crumbles.transform.parent = gameObject.transform.parent;
        Destroy(gameObject);
    }
}
