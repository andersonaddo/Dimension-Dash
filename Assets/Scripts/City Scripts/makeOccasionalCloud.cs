using UnityEngine;
using System.Collections;

public class makeOccasionalCloud : MonoBehaviour {

    Transform player;
    public GameObject cloud, gameCamera;

	// Use this for initialization
	void Start () {
        player = gameCamera.GetComponent<followCharacter>().character;
	}
	
	// Update is called once per frame
	void Update () {

        //Making an occasional cloud of player is close enough
        if (Vector2.Distance(player.position, transform.position) < 50) {

            if (Random.Range(0, 26) < 2)
            {
                Instantiate(cloud,
                            new Vector3(transform.position.x + Random.Range(-1, 1.1f), transform.position.y + Random.Range(-1, 1.1f), 0),
                            transform.rotation);             
            }

        //Moving the generator forward
        transform.position = new Vector3(transform.position.x + 1.9f, transform.position.y, transform.position.z);

        }
	
	}
}
