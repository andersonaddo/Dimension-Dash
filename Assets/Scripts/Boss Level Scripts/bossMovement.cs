using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bossMovement : MonoBehaviour {

    bossHealth myBH;
    bossShooting myBS;

    [HideInInspector]
    public bool isTired;
    bool goDown;
    [HideInInspector]
    public bool isCharging;
    bool isReturning;

    public Transform downPostiton; //Where the boss will land when tired
    Vector3 regularPosition; //Where the boss will be when he's not tired
    public Transform characterBasePosition;

    public GameObject warningText;

    public float returnRate; //How fast he returns to his original position
    public float chargingSpeed; //How fast he charges



    // Use this for initialization
    void Start () {
        myBH = GetComponent<bossHealth>();
        myBS = GetComponent<bossShooting>();
    
        regularPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //If tired, start the charge coroutine
	    if (isTired)
        {
            StartCoroutine("charge");
        }

        //Dictating where to go when the boss is dropping down
        if (goDown) transform.position = Vector2.Lerp(transform.position, downPostiton.position, returnRate * Time.deltaTime);

        //Dictating where to go when the boss is charging
        if (isCharging) transform.position = Vector2.MoveTowards(transform.position, new Vector2(characterBasePosition.position.x, downPostiton.position.y), chargingSpeed);

        //Dictating where to go when the boss is going baack to his original position
        if (isReturning) transform.position = Vector2.Lerp(transform.position, regularPosition, returnRate * Time.deltaTime);

    }

    public void makeTired()
    {
        isTired = true;
    }

    IEnumerator charge()
    {
        //Disabling the boss from shooting
        myBS.isMoving = true;

        isReturning = false;
        isTired = false;

        yield return new WaitForSeconds(3);

        //Going down
        goDown = true;

        //Waiting untill the boss is low enough
        yield return new WaitUntil( ()=> Vector2.Distance(transform.position, downPostiton.position) < 0.01);
        
        //Flashing the warning sign twice before the charge
        for (int i = 0; i < 2; i++)
        {
            warningText.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            warningText.SetActive(false);
            yield return new WaitForSeconds(0.15f);
        }
        goDown = false;

        //Making the boss damageable (check the bossHealth to see more)
        myBH.isDamageable = true;

        //Charging
        isCharging = true;

        //Waiting untill the boss is finished charging
        yield return new WaitUntil(() => Vector2.Distance(transform.position, new Vector2(characterBasePosition.position.x, downPostiton.position.y)) < 0.01);

        isCharging = false;

        //Waiting a bit to give the character a chance to hit the boss
        yield return new WaitForSeconds(0.75f);

        //Returning to the regular position
        goDown = true;
        yield return new WaitForSeconds(0.5f);
        goDown = false;
        isReturning = true;

        //Waiting unitll the boss is back at original position to allow shooting (if the boss is still alive)
        yield return new WaitUntil(() => Vector2.Distance(transform.position, regularPosition) < 0.01);

        if (!myBH.isDead)
        {
            myBS.isMoving = false;
        }else{
            myBH.makeDead();
        }
       
    }
}

