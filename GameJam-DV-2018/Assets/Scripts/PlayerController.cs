using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float projectileSpeed;
	[SerializeField] private GameObject projectilePrefab;
	private Rigidbody2D playerRig;
    private Vector3 playerOrigPos;
	private Vector3 lastDirection = Vector3.right;

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

		if (!V3Equal(moveDirection, Vector3.zero)) {
			lastDirection = moveDirection.normalized;
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
		if (Input.GetButtonDown("Fire1"))
		{
			GameObject projectile = Instantiate(projectilePrefab, transform.position + lastDirection, transform.rotation) as GameObject;
			projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(lastDirection.x , lastDirection.y) * projectileSpeed;
		}

	}

	public bool V3Equal(Vector3 a, Vector3 b)
	{
		return Vector3.SqrMagnitude(a - b) < 0.00000001;
	}



}
