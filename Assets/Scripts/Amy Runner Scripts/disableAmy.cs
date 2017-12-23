using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used when the player is being sucked into the endgame portal
/// </summary>
public class disableAmy : MonoBehaviour {

    public GameObject portalCenter;

    //Suction variables
    public float suctionForce;
    GameObject objectBeingPulled;

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<amyMovement>().enabled = false;
        portalCenter.GetComponent<teleportAmyOut>().player = other.gameObject;
        portalCenter.GetComponent<teleportAmyOut>().getReady();

        //Setting up for suction
        objectBeingPulled = other.transform.parent.gameObject;
        //Adding the endgame animation
        other.gameObject.GetComponent<Animator>().SetBool("isDashing", true);
    }

    //Sucking Amy's PARENT to the portal's center (since the child's animation's credentials has prevented direct movement)
    void OnTriggerStay2D(Collider2D other)
    {
        //Checking if the gameObject is the player
        if (other.gameObject.tag == "Player")
        {
            //Suck the player in
            objectBeingPulled.transform.position = Vector3.MoveTowards
                (objectBeingPulled.transform.position,
                 transform.position,
                 suctionForce * Time.deltaTime);
        }
    }

}
