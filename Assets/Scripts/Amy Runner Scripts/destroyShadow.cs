using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyShadow : MonoBehaviour {

    public GameObject shadow;

	void OnTriggerEnter2D()
    {
        Destroy(shadow);
    }
	
}
