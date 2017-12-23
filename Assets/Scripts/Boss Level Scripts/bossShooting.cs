using UnityEngine;
using System.Collections;

public class bossShooting : MonoBehaviour {

    bossMovement myBM;
    public bool isMoving; //Is the boss moving?

    public GameObject fireBall;
    
    ArrayList usedShootPatterns = new ArrayList (); //The shooting patterns that have been used since the boss was last tired

    public int maxBulletCount; //The max amount of bullets the boss can shoot in any shooting session
    int minBulletCount = 1; //The min amount of bullets the boss can shoot in any shooting session

    bool canShoot = true; //Can the boss shoot now?

    public int maxShootSessionCount; //The max bullet patterns shot before the next tired session
    int sessionCount; //Number of bullets that have been shot isince the last tired session

    public float restTime; //The interval between shooting sessions
    float nextShootTime; //The next time a shooting session will occur

    int currentPattern;


	// Use this for initialization
	void Start () {

        addWaitTime();

        myBM = GetComponent<bossMovement>();

        //Setting up the shoot patterns accopording to how many times the boss has been encountered
        //(because the boss has a difficulty curve)
        setUpBulletLimits();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time > nextShootTime && canShoot && !isMoving)
        {
            getShootPattern();
            canShoot = false;
        }

        //If we've reached the maximum number of shooting sessions, get tired
        if (sessionCount == maxShootSessionCount)
        {
            getTired();
        }


	}

    void getShootPattern ()
    {
        bool canUse;

        //Getting a shoot pattern that hasn't already been used
        while (true)
        {
            canUse = true;
            currentPattern = Random.Range(minBulletCount, maxBulletCount);
            
            foreach (int i in usedShootPatterns)
            {
                if (i == currentPattern) canUse = false;
            }
            if (canUse) break;
        }

            //Then, shooting
            if (canUse)
            {
                usedShootPatterns.Add(currentPattern); //Adding this pattern to the used list
                StartCoroutine("shoot");
            }
        
    }


    IEnumerator shoot()
    {
        float interval = Random.Range(0.4f, 0.7f);
        for (int i = 0; i<= currentPattern; i++)
        {
            //Instanciating a fireball at regular intervals
            //The amount of fireballs made in this session equals the value of the currentPattern variable,
            //which was set by getShootPattern()

            Instantiate(fireBall, transform.position, Quaternion.Euler(0, 0, 0));
           
            yield return new WaitForSeconds(interval);
        }
        addWaitTime();
        sessionCount++; //Incrementing how many shooting sessions have gone on
        canShoot = true;
    }

    void getTired()
    {
        //Everytime the boss gets tired, the ne essary variable reset.
        myBM.makeTired();
        usedShootPatterns = new ArrayList();
        sessionCount = 0;
    }

    public void addWaitTime()
    {
        nextShootTime = Time.time + restTime;
    }



    //Called at the beginning to make the boss harder if he's been met more than once
    void setUpBulletLimits()
    {
        switch (globalDataPreserver.Instance.encounters)
        {
            case 1:
                increaseLimitsBy(0);
                break;
            case 2:
                increaseLimitsBy(2);
                break;
            case 3:
                increaseLimitsBy(3);
                break;
            case 4:
                increaseLimitsBy(4);
                break;
            case 5:
                increaseLimitsBy(5);
                break;
            default:
                increaseLimitsBy(5);
                break;
        }
    }



    void increaseLimitsBy(int increment)
    {
        minBulletCount += increment;
        maxBulletCount += increment;
    }
}
