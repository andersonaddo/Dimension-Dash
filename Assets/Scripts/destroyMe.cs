using UnityEngine;
using System.Collections;

public class destroyMe : MonoBehaviour {

    public float aliveTime = 5;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, aliveTime);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
