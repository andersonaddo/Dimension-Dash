using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallSpeed : MonoBehaviour {

    Rigidbody2D myRB;
    public float speed;

    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        setUpSpeed();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRB.velocity = new Vector2(speed, myRB.velocity.y);
    }

    //Setting up the speed of the fireballs accoring to how many times the boss has been encountered
    //The slower the speed, the harder the ball is to dodge. Keep in mind that they're faster when the speed variable is slower
    void setUpSpeed()
    {
        switch (globalDataPreserver.Instance.encounters)
        {
            case 1:
                speed += 0.3f;
                break;
            case 2:
                speed += 1.3f;
                break;
            case 3:
                speed += 2.6f;
                break;
            case 4:
                speed += 3.1f;
                break;
            case 5:
                speed += 3.8f;
                break;
            default:
                speed += 3.8f;
                break;
        }
    }
}
