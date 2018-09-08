using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBaseController {
    
    public float sightAngle = 120f;
    public float sightDist = 100f;
    public LayerMask wall;
    public Transform target;


    bool TargetInFieldOfView()
    {
        Vector3 targetDir = target.position - transform.position;
        Vector3 forward = transform.position - origPos;
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

    protected override void Die()
    {
        Destroy(gameObject, 0);
    }

    protected override void LoseLife()
    { 
    }

    protected override void CharacterUpdate()
    {
        if(target != null)
        {
            Vector3 moveDirection = Vector3.right;
            UpdateRotation();
            if (TargetInFieldOfView() && TargetWithClearVision())
            {
                moveDirection = target.position - gameObject.transform.position;
                if (TargetWithClearVision())
                {
                    Fire();
                }
            }
            else
                moveDirection = gameObject.transform.position - origPos;

            //Update speed based on the directional movement and the defined movement speed
            rig.velocity = moveDirection.normalized * speed;
        }
    }
}
