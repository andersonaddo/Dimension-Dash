using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// The scripts that manages the apparent movement of the stationary character
/// </summary>
public class stationaryCharacter : MonoBehaviour {

    public Transform basePosition; //The position where the character will be based
    public float returnRate; //How fast the character will return to the base position

    //Speed and physics variables
    Rigidbody2D myRB;
    Animator myAnim;

    //Stamina variables
    public float maxStamina;
    public int dashStamina, smashStamina, doubleJumpStamina;
    public Slider staminaSlider;
    public float staminaRate;
    float nextStaminaRefil;
    float currentStamina;

    //Jumping variables
    [HideInInspector]
    public bool isGrounded;
    public float jumpSpeed;
    //Double jump variables
    public GameObject blastSource;
    [HideInInspector]
    public bool hasDoubleJumped;
    public GameObject doubleJumpPS;
    public float doubleJumpDivider;

    //Dashing variables    
    [HideInInspector]
    public bool isDashing;
    public float dashRate;
    float nextDashTime = 0;
    
    public float dashSpeed;
    public float dashingDuration;
    float dashStopTime;
    

    //Smashing varibles
    float smashRate = 1;
    float nextSmashtime = 0;
    public float smashForce;
    [HideInInspector]
    public bool isSmashing;

    //Touch Variables
    private Vector3 firstPos;   //First touch position
    private Vector3 lastPos;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    bool isTouchValid = true;





    // Use this for initialization
    void Start()
    {
        //Putting the character at the base position
        transform.position = new Vector2(basePosition.position.x, transform.position.y);

        myAnim = GetComponent<Animator>();
        myRB = GetComponent<Rigidbody2D>();

        //Initializing the slider
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = staminaSlider.maxValue;
        currentStamina = maxStamina;

        //Initializing touch value
        dragDistance = Screen.height * 10 / 100; //dragDistance is 10% height of the screen
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

        //Constantly updating the animator variables
        myAnim.SetFloat("verticalSpeed", myRB.velocity.y);

        //Increasing stamina periodically
        if (Time.time > nextStaminaRefil && currentStamina < maxStamina)
        {
            currentStamina += 1;
            nextStaminaRefil = Time.time + staminaRate;
        }

        //Managing the dashing
        if (isDashing)
        {
            if (Time.time > dashStopTime)
            {
                myRB.velocity = new Vector2(0, myRB.velocity.y);
                myAnim.SetBool("isDashing", false);
                isDashing = false;
            }
            else
            {
                myRB.velocity = new Vector2(dashSpeed, myRB.velocity.y);
            }
        }

        //Moving the character back to the base position if not dashing but the character is off the base position
        if (isGrounded && !isDashing && transform.position.x != basePosition.position.x + 0.01)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(basePosition.position.x, -3.233999f), returnRate * Time.deltaTime);
        }


    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Resetting the bool values to prepare the next jump
            isGrounded = true;
            isSmashing = false;
            hasDoubleJumped = false;
            myAnim.SetBool("isGrounded", true);
            myAnim.SetBool("isSmashing", false);

            
        }
    }


    //Everytime you land, increase the score
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            increaseScore(2);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Resetting the bool values to prepare the next jump
            isGrounded = true;
            isSmashing = false;
            hasDoubleJumped = false;
            myAnim.SetBool("isGrounded", true);
            myAnim.SetBool("isSmashing", false);


        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Player is no longer grounded
            isGrounded = !isGrounded;
            myAnim.SetBool("isGrounded", false);
        }
    }


    public void jump()
    {
        if (isGrounded)
        {
            //If the player is jumping from the ground, do a standard jump
            myRB.AddForce(new Vector2(0, 1) * jumpSpeed, ForceMode2D.Impulse);
            return;
        }

        //Else if he is in the air but he hasn't double jumped and...
        else if (!isGrounded && !hasDoubleJumped && currentStamina >= doubleJumpStamina)
        {

            //Adding the double-jump PS
            Instantiate(doubleJumpPS, blastSource.transform.position, transform.rotation);

            myRB.velocity = new Vector2(myRB.velocity.x, 0);
            myRB.AddForce(new Vector2(0, 1) * (jumpSpeed / doubleJumpDivider), ForceMode2D.Impulse);//Jump
            hasDoubleJumped = true;

            //Reducing stamina accoringly
            currentStamina -= doubleJumpStamina;

            //Increasing the score
            increaseScore(5);

            return;
        }
    }


    //Called when player dashes
    void dash()
    {
        if (currentStamina >= dashStamina && Time.time > nextDashTime)
        {

            dashStopTime = Time.time + dashingDuration;
        
            isDashing = true;
            myAnim.SetBool("isDashing", true);

            //Reducing stamina accoringly
            currentStamina -= dashStamina;

            //Increasing the score
            increaseScore(10);

            nextDashTime = Time.time + dashRate;
        }
    }


    //Called when player smashes
    void smash()
    {
        if (currentStamina >= smashStamina && Time.time > nextSmashtime && !isGrounded)
        {
            //Resetting speed, stopping in mid air for a split second, and smashing down.
            myRB.velocity = new Vector2(myRB.velocity.x, 0);
            myRB.AddForce(new Vector2(0, -smashForce), ForceMode2D.Impulse);
            myAnim.SetBool("isSmashing", true);

            isSmashing = true;

            //Reducing stamina accoringly
            currentStamina -= smashStamina;

            //Increasing the score
            increaseScore(30);

            nextSmashtime = Time.time + smashRate;
        }
    }



    //Called to access script of global data preserver gameobject and increase the score
    void increaseScore(int increment)
    {
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

                //Check if drag distance is greater than 20% of the screen height. If it is, register a drag
                if ((Mathf.Abs(lastPos.x - firstPos.x) > dragDistance || Mathf.Abs(lastPos.y - firstPos.y) > dragDistance) && isTouchValid)
                {
                    isTouchValid = false;

                    //Check if the drag is vertical or horizontal
                    if (Mathf.Abs(lastPos.x - firstPos.x) > Mathf.Abs(lastPos.y - firstPos.y))
                    {
                        if (lastPos.x > firstPos.x) dash(); //Right Swipe
                    }
                    else
                    {
                    
                        if (lastPos.y < firstPos.y) smash();  //Downward Swipe
                        else jump();//Upwards Swipe
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
