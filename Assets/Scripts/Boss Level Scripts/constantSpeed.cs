using UnityEngine;
using System.Collections;

public class constantSpeed : MonoBehaviour {

    Rigidbody2D myRB;
    public float speed;

	// Use this for initialization
	void Start () {
        myRB = GetComponent<Rigidbody2D>();
        myRB.velocity = new Vector2(speed, myRB.velocity.y);
    }

    //Can be called externally by a broadcast or something to stop the moving object
    public void stop()
    {
        myRB.velocity = new Vector2(0, myRB.velocity.y);
    }
}
