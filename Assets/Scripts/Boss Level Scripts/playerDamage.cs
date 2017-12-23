using UnityEngine;
using System.Collections;

public class playerDamage : MonoBehaviour {

    Material playerMaterial;

    //UI variables
    bool canBringPanel = true;
    public GameObject deathPanel, pauseButton, basicPanel;

    public int fireBallDDamage;
    public int bossDamage;

    public GameObject fireBallPS;

    bool isDead;

	// Use this for initialization
	void Start () {
        playerMaterial = transform.parent.GetComponent<Renderer>().material;

        prepareDamageValues();
    }


    void Update()
    {
        //Adding a constant clearing effect to the character's color.
        //It's effect will only be seen if addDamage() is called.
        if (!isDead)
          playerMaterial.color = Color.Lerp(playerMaterial.color, Color.white, 0.09f);
        
            

        //Preventing the score from going to the negatives.
        //This also ensures that once the score reaches 0, it stays that way
        //Also fading th player away if isDead and bringing the death panel up.
        if (isDead)
        {
            globalDataPreserver.Instance.playerScore = 0;
            playerMaterial.color = Color.Lerp(playerMaterial.color, Color.clear, 0.02f);   
            if (playerMaterial.color.a < 0.01 && canBringPanel)
            {
                pauseButton.SetActive(false);
                basicPanel.SetActive(false);
                deathPanel.SetActive(true);
                Time.timeScale = 0;
                canBringPanel = false;
            }         
        }
    }



    //Everytime a weapon hits you, decrease the score
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            addDamage(fireBallDDamage);

            //Making the explosion and deleting the fireball.
            Instantiate(fireBallPS, other.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(other.gameObject);
        }

        //Checking if being hit by the charging boss
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.gameObject.GetComponent<bossMovement>().isCharging)
            {
                addDamage(bossDamage);
            }
        }
        
    }


    void addDamage(int damage)
    {
        if (!isDead)
        {
            globalDataPreserver.Instance.playerScore -= damage;

            //Quickly altering the character's color
            playerMaterial.color = Color.red;
        }
       
        if (globalDataPreserver.Instance.playerScore <= 0) isDead = true;
    }


    void prepareDamageValues()
    {

        //Determining how hard the boss should hit according to how many times he's been encountered
        switch (globalDataPreserver.Instance.encounters)
        {
            case 1:
                addToDamages(13, 0);
                break;
            case 2:
                addToDamages(22, 150);
                break;
            case 3:
                addToDamages(27, 200);
                break;
            case 4:
                addToDamages(63, 1000);
                break;
            case 5 :
                addToDamages(130, 10000);
                break;
            default:
                addToDamages(170, 10000);
                break;
        }           
    }

    void addToDamages(int fireBall, int boss)
    {
        fireBallDDamage += fireBall;
        bossDamage += boss;
    }
}
