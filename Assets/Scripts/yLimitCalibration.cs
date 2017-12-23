using UnityEngine;
using System.Collections;

public class yLimitCalibration : MonoBehaviour {

    public float difference;
    public GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Constantly cchangin y in respect to target
	    transform.position = new Vector3(target.transform.position.x, target.transform.position.y + difference, target.transform.position.z);
    }
	
        
}
