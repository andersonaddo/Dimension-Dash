using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is added to coins
/// </summary>
public class coinCountIncrementer : MonoBehaviour
{
    public GameObject particles;
    public int amountAdded = 1; //Can be changed in Inspector if needed

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter2D()
    {
        Instantiate(particles, transform.position, Quaternion.Euler(0, 0, 0));
        globalDataPreserver.Instance.incrementCoinCount(amountAdded);
        Destroy(gameObject, 2);
        GetComponent<AudioSource>().Play();
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.clear;
    }
}
	
