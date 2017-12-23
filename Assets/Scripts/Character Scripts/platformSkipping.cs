using UnityEngine;
using System.Collections;

/// <summary>
/// This script allows players to register when they've skipped over a platform or not (to earn a score multiplier)
/// </summary>
public class platformSkipping : MonoBehaviour {

    bool hasSkipped;
    public characterMovement character;
    [HideInInspector]public bool hasBonus;
    public GameObject bonusGUI;
    Animator GuiAnim;

    public float bonusDuration;
    float bonusEndTime;

	// Use this for initialization
	void Start () {
        character = GetComponentInParent<characterMovement>();
        hasBonus = false;
        GuiAnim = bonusGUI.GetComponent<Animator>();
	}
	
    void Update()
    {
        if (Time.time > bonusEndTime && hasBonus)
        {
            hasBonus = false;
            GuiAnim.SetBool("hasBonus", false);
        }
    }


    //You haven't skipped the the platform if you landed on the platform
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && character.isGrounded) hasSkipped = false;
    }

    //If you leave a platform and you havent landed...
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && hasSkipped)
        {
            bonusEndTime = Time.time + bonusDuration;
            GuiAnim.SetBool("hasBonus", true);
            hasBonus = true;
        }
    }
        

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) hasSkipped = true; //Resetting
    }
    }
