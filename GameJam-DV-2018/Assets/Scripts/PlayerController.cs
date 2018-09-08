using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float projectileSpeed;
	[SerializeField] private GameObject laserPrefab;
	private Rigidbody2D playerRig;
    private Vector3 playerOrigPos;
	private Vector3 lastDirection;

	private void Awake()
    {
        playerRig = this.GetComponent<Rigidbody2D>();
        //Set original position
        playerOrigPos = gameObject.transform.position;

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Obtain direction vector
        Vector3 moveDirection = gameObject.transform.position - playerOrigPos;

        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

		if (!moveDirection.Equals(Vector3.zero) && !moveDirection.Equals(lastDirection)) {
			lastDirection = moveDirection;
		}

        //Obtain directional movement defined on the inputs
        float verticalSpeed = Input.GetAxis("Vertical");
        float horizontalSpeed = Input.GetAxis("Horizontal");
        //Update speed based on the directional movement and the defined movement speed
        playerRig.velocity = new Vector2(horizontalSpeed, verticalSpeed) * speed;
        // shoot a projectile
		fire();
		//Update original position
		playerOrigPos = gameObject.transform.position;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collisiono");
    }

	public void fire()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(lastDirection.x , lastDirection.y) * projectileSpeed;
		}

	}



}
