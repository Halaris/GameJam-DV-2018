using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {

	[SerializeField] private AudioSource lifeDeployAS;
	[SerializeField] private AudioSource lifeCollAS;

	// Use this for initialization
	void Start () {
		Destroy(Instantiate(lifeDeployAS, transform.position, transform.rotation),1);
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
			Destroy(Instantiate(lifeCollAS, transform.position, transform.rotation), 1);
			Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
