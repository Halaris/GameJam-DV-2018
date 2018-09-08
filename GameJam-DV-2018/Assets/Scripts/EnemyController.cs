using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    Rigidbody2D rig;
    Vector3 _origPos;

    public Rigidbody2D bullet;
    public float speed = 50f;
    Vector3 moveDirection = Vector3.right;
    public float sightAngle = 120f;
    public float sightDist = 100f;
    public LayerMask wall;
    public Transform target;
    private void Awake()
    {
        rig = this.GetComponent<Rigidbody2D>();
        //Set original position
        _origPos = gameObject.transform.position;

    }

    // Use this for initialization
    void Start()
    {

    }


    void FireBullet()
    {
        Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(bullet, transform.position, transform.rotation);
        bulletClone.velocity = rig.velocity;

        // You can also access other components / scripts of the clone
        //bulletClone.GetComponent<MyRocketScript>().DoSomething();
    }
    bool TargetInFieldOfView()
    {
        Vector3 targetDir = target.position - transform.position;
        Vector3 forward = transform.position - _origPos;
        float enemyAngle = Vector3.Angle(targetDir, forward);
        var dist = Vector3.Distance(target.position, this.transform.position);
        return (enemyAngle < sightAngle && dist < sightDist);
    }
    bool TargetWithClearVision()
    {
        var dirToTarget = (target.position - transform.position).normalized;
        var dist = Vector3.Distance(target.position, this.transform.position);
        return !Physics2D.Raycast(transform.position, dirToTarget, dist, wall);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        Vector3 forward = transform.position - _origPos;
        float enemyAngle = Vector3.Angle(targetDir, forward);
        var dist = Vector3.Distance(target.position, this.transform.position);
        if (TargetInFieldOfView())
        {
            moveDirection = target.position - gameObject.transform.position;
            if(TargetWithClearVision())
            {
                //Shoot
            }
        }
        else
            moveDirection = gameObject.transform.position - _origPos;

        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        //Obtain directional movement defined on the inputs
        float verticalSpeed = Random.Range(-1, 1);
        float horizontalSpeed = Random.Range(-1, 1);
        //Update speed based on the directional movement and the defined movement speed
        rig.velocity = moveDirection.normalized * speed;
        //Update original position
        _origPos = gameObject.transform.position;
    }
}
