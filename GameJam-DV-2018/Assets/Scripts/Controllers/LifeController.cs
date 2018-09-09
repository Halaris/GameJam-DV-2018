using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.lives < 3)
            {
                player.lives++;
                player.updateLife();
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
