using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class characterMovement : MonoBehaviour
{

    //Variables for death
    public float minY; //The lowest the character can go before considered dead
    public Camera gameCamera;
    
    //These booleans are acutally misleading in their names
    //If isSpaceLevel or isCityLevel is true, that means the character has a dedicated double-jump animation to play
    //if isSkyLevel is true, then the player will just emit particles when they double jump
    public bool isSpaceLevel, isSkyLevel, isCityLevel;

    //Speed and physics variables
    public float baseSpeed;
    Rigidbody2D myRB;
    [HideInInspector]
    public Animator myAnim;
    [HideInInspector]
    public float currentSpeed;

    //Stamina variables
    public float maxStamina;
    public int dashStamina, smashStamina, doubleJumpStamina;
    public Slider staminaSlider;
    public float staminaRate;
    float nextStaminaRefil;
    [HideInInspector]
    public float currentStamina;

    //Jumping variables
    [HideInInspector]
    public bool isGrounded;
    public float jumpSpeed;
    //Double jump variables
    public GameObject blastSource;
    [HideInInspector]
    public bool hasDoubleJumped;
    public GameObject doubleJumpPS;


    //Dashing variables
    public float dashingDuration;
    float dashStopTime = 0;
    float dashSpeed;
    bool isDashing;
    public float dashRate;
    float nextDashTime = 0;
    public float doubleJumpDivider;

    //Smashing varibles
    float smashRate = 0.5f;
    float nextSmashtime = 0;
    public float smashForce;

    //Speed incrementation variables
    float nextIncrementTime = 0;
    public float incrementRate;
    public float maxSpeed;

    //Touch Variables
    private Vector3 firstPos;   //First touch position
    private Vector3 lastPos;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    bool isTouchValid = true;


    public GameObject skipper;
    platformSkipping myPK;







    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = staminaSlider.maxValue;
        currentStamina = maxStamina;
        dashSpeed = baseSpeed * 5f;
        currentSpeed = baseSpeed;

        myAnim = GetComponent<Animator>();

        //Initializing touch value
        dragDistance = Screen.height * 10 / 100; //dragDistance is 10% height of the screen

        myPK = skipper.GetComponent<platformSkipping>();
    }

    void Update()
    {
#if UNITY_EDITOR
        doKeyboardControls();
#else
        doTouchCalculations(); 
#endif


        //Constantly updating stamina slider
        staminaSlider.value = currentStamina;

        //Incrememting speed
        if (Time.time > nextIncrementTime && baseSpeed < maxSpeed)
        {
            baseSpeed += 0.1f;
            nextIncrementTime = Time.time + incrementRate;
        }

        //Checking if it's time to terminate dash (if dashing in the first place)   
        if (isDashing)
        {
            if (Time.time > dashStopTime)
            {
                currentSpeed = baseSpeed;
                myAnim.SetBool("isDashing", false);
                isDashing = false;
            }
        }

        //Increasing stamina periodically
        if (Time.time > nextStaminaRefil && currentStamina < maxStamina)
        {
            currentStamina += 1;
            nextStaminaRefil = Time.time + staminaRate;
        }

        //Killing the player if he's too low
        if (transform.position.y < minY) gameCamera.GetComponent<followCharacter>().isDead = true;

    }

    // LateUpdate is called once per frame after Update
    void FixedUpdate()
    {

        //Constantly updating the animator variables
        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);


        myRB.velocity = new Vector2(1 * currentSpeed, myRB.velocity.y); //Giving the character a constant speed

        if (!isDashing)
        {
            myRB.velocity = new Vector2(1 * baseSpeed, myRB.velocity.y); //Giving the character a constant speed
        }

    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Resetting the bool values to prepare the next jump
            isGrounded = true;
            hasDoubleJumped = false;
            myAnim.SetBool("isGrounded", true);
            myAnim.SetBool("isSmashing", false);
            if (isSpaceLevel || isCityLevel) myAnim.SetBool("isDoubleJumping", false);

        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //Everytime you land, increase the score
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            increaseScore(10);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Resetting the bool values to prepare the next jump
            isGrounded = true;
            hasDoubleJumped = false;
            myAnim.SetBool("isGrounded", true);
            myAnim.SetBool("isSmashing", false);
            if (isSpaceLevel || isCityLevel) myAnim.SetBool("isDoubleJumping", false);

        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Player is no longer grounded
            isGrounded = false;
            myAnim.SetBool("isGrounded", false);
            //Destroying clouds that have been stepped on
            Destroy(other.gameObject, 7f);
        }
    }


    public void jump()
    {

        if (isGrounded)
        {
            //If the player is jumping from the ground, do a standard jump
            myRB.velocity = new Vector2(0, 0);
            myRB.AddForce(new Vector2(0, 1) * jumpSpeed, ForceMode2D.Impulse);
            return;
        }

        //Else if he is in the air but he hasn't double jumped and...
        else if (!isGrounded && !hasDoubleJumped && currentStamina >= doubleJumpStamina)
        {

            if (isSkyLevel) Instantiate(doubleJumpPS, blastSource.transform.position, transform.rotation);
            if (isSpaceLevel || isCityLevel) myAnim.SetBool("isDoubleJumping", true);

            myRB.velocity = new Vector2(myRB.velocity.x, 0);
            myRB.AddForce(new Vector2(0, 1) * (jumpSpeed / doubleJumpDivider), ForceMode2D.Impulse);//Jump
            hasDoubleJumped = true;

            //Reducing stamina accoringly
            currentStamina -= doubleJumpStamina;

            //Increasing the score
            increaseScore(40);

            return;

        }

    }


    //Called when player dashes
    void dash()
    {
        if (currentStamina >= dashStamina && Time.time > nextDashTime)
        {
            currentSpeed = dashSpeed;
            dashStopTime = Time.time + dashingDuration;
            isDashing = true;
            myAnim.SetBool("isDashing", true);

            //Reducing stamina accoringly
            currentStamina -= dashStamina;

            //Increasing the score
            increaseScore(70);

            nextDashTime = Time.time + dashRate;
        }
    }


    //Called when player smashes
    void smash()
    {
        if (currentStamina >= smashStamina && Time.time > nextSmashtime && !isGrounded)
        {
            //Resetting speed, stopping in mid air for a split second, and smashing down.
            currentSpeed = baseSpeed;
            myRB.velocity = new Vector2(myRB.velocity.x, 0);
            myRB.AddForce(new Vector2(0, -smashForce), ForceMode2D.Impulse);
            myAnim.SetBool("isSmashing", true);

            //Reducing stamina accoringly
            currentStamina -= smashStamina;

            //Increasing the score
            increaseScore(60);

            nextSmashtime = Time.time + smashRate;
        }
    }



    //Called to access script of global data preserver gameobject and increase the score
    public void increaseScore(int increment)
    {
        if (myPK.hasBonus) increment *= 3;

        globalDataPreserver.Instance.playerScore += increment;
    }


    void doTouchCalculations()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                firstPos = touch.position;
                lastPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lastPos = touch.position;

                //Check if drag distance is greater than 20% of the screen height. If it is, then register a drag
                if ((Mathf.Abs(lastPos.x - firstPos.x) > dragDistance || Mathf.Abs(lastPos.y - firstPos.y) > dragDistance) && isTouchValid)
                {
                    isTouchValid = false;

                    if (Mathf.Abs(lastPos.x - firstPos.x) > Mathf.Abs(lastPos.y - firstPos.y))
                    {         
                        if (lastPos.x > firstPos.x) dash(); //Right swipe
                    }
                    else
                    {
                        
                        if (lastPos.y < firstPos.y) smash(); //Downwards Swipe

                        else jump();//Upwards SwipeS
                    }

                }
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                isTouchValid = true;
            }
        }
    }

    void doKeyboardControls()
    {
        if (Input.GetButtonDown("Jump")) jump();

        if (Input.GetButtonDown("Dash")) dash();

        if (Input.GetButtonDown("Smash")) smash();
    }

}
