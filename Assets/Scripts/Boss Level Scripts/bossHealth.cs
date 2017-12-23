using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bossHealth : MonoBehaviour {

    //Death varibales
    [HideInInspector]
    public bool isBlinking; //Called when the boss is blinking to start the fading
    [HideInInspector]
    public bool isDead = false;
    public GameObject portal;
    public Transform portalPosition;
    public GameObject player;
    public GameObject coinPS;


    Material bossMaterial;
    public Slider healthSlider;

    //Health variables
    public int maxHealth;
    
    //The boss has two trigger colliders due to it's complex shape, so due to is the player smashes through the boss,
    //OnTriggerEnter2D will be called twice. This variable is here to prevent that.
    [HideInInspector]
    public bool isDamageable;


	// Use this for initialization
	void Start () {

        //Giving the boss slightly less health if he's been ecnountered several times
        reduceHealth();

        bossMaterial = GetComponent<Renderer>().material;
        healthSlider.value = 0;
        healthSlider.maxValue = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {

        //Adding a constant clearing effect to the boss' color.
        //It's effect will only be seen if addDamage() is called. 
        if (!isBlinking)
        {
            bossMaterial.color = Color.Lerp(bossMaterial.color, Color.white, 0.09f);
        }else{
            //Fading the boss as he blinks
            bossMaterial.color = Color.Lerp(bossMaterial.color, Color.clear, 0.02f);
            if (bossMaterial.color.g < Color.clear.g + 0.03f)
            {
                //Making the portal to take ple player back to the running dimensions
                GameObject madePortal = Instantiate(portal, portalPosition.position, Quaternion.Euler(0, 0, 90));
                madePortal.GetComponent<teleportOutBoss>().player = player;

                Destroy(gameObject);
            }
        }

    }


    //Checking if the player is smashing onto the boss, and damaging the boss if he is.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Damage Sensor") && isDamageable)
        {
            if (other.gameObject.transform.parent.GetComponent<stationaryCharacter>().isSmashing)
            {
                addDamage();
            }
        }
    }

    //Called to damage the boss
    void addDamage()
    {
        healthSlider.value++;

        //Quickly altering the character's color
        bossMaterial.color = Color.red;

        //Preventing the boss from being hurt twice due to the presence of two colliders
        isDamageable = false;

        //Killing the boss if the health is depleted
        if (maxHealth == healthSlider.value)
        {
            isDead = true;
        }
    }

    //Called when the boss' health is depleted and he is back at his original position
    public void makeDead()
    {
        coinPS.SetActive(true);

        //Calculating the coins awarded
        int reward = 40;
        switch (globalDataPreserver.Instance.encounters)
        {
            case 2:
                reward = 100;
                break;
            case 3:
                reward = 150;
                break;
            case 4:
                reward = 170;
                break;
            case 5:
                reward = 273;
                break;
            default:
                reward = 350;
                break;
        }

        globalDataPreserver.Instance.incrementCoinCount(reward);


        isBlinking = true;
        InvokeRepeating("blink", 0.01f, 0.05f);

    }

    void blink()
    {
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
    }

    void reduceHealth()
    {
        switch (globalDataPreserver.Instance.encounters)
        {
            case 3:
                maxHealth -= 2;
                break;
            case 4:
                maxHealth -= 2;
                break;
            case 5:
                maxHealth -= 3;
                break;
        }
    }

}
