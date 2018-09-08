using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBaseController
{

    public float sightAngle = 120f;
    public float sightDist = 100f;
    public LayerMask wall;
    public LayerMask wayPoints;
    public Transform target;
    [SerializeField] private Transform lastVisitedPoint;

    bool TargetInFieldOfView(Transform targetPoint, float viewAngle, float viewDist)
    {
        Vector3 targetDir = targetPoint.position - transform.position;
        Vector3 forward = transform.position - origPos;
        float enemyAngle = Vector3.Angle(targetDir, forward);
        var dist = Vector3.Distance(targetPoint.position, this.transform.position);
        return (enemyAngle < viewAngle && dist < viewDist);
    }
    bool TargetWithClearVision(Transform targetPoint)
    {
        Vector3 dirToTarget = (targetPoint.position - transform.position).normalized;
        float dist = Vector3.Distance(targetPoint.position, this.transform.position);
        return !Physics2D.Raycast(transform.position, dirToTarget, dist, wall);
    }

    protected override void Die()
    {
        Destroy(gameObject, 0);
    }

    protected override void LoseLife()
    {
    }
    private Vector3 SearchWayPoint(Vector3 moveDirection)
    {
        //moveDirection = gameObject.transform.position - origPos;
        Vector3 forward = transform.position - origPos;
        SortedDictionary<double, Transform> visiblePoints = new SortedDictionary<double, Transform>();
        Collider2D[] wayPointsInViewRadius = Physics2D.OverlapCircleAll(transform.position, sightDist, wayPoints);
        foreach (Collider2D wayPointInFieldOfView in wayPointsInViewRadius)
        {
            Transform targetPoint = wayPointInFieldOfView.transform;
            if (targetPoint != lastVisitedPoint && TargetInFieldOfView(targetPoint, sightAngle, sightDist) && TargetWithClearVision(targetPoint))
            {
                float dist = Vector3.Distance(targetPoint.position, transform.position);
                if (dist < 1)
                {
                    lastVisitedPoint = targetPoint;
                }
                else
                {
                    visiblePoints.Add(dist, targetPoint);
                }
            }
        }
        SortedDictionary<double, Transform>.Enumerator numerator = visiblePoints.GetEnumerator();
        if (numerator.MoveNext())
        {
            return numerator.Current.Value.position - transform.position;
        }
        return moveDirection;
    }
    protected override void CharacterUpdate()
    {
        if (target != null)
        {
            Vector3 moveDirection = Vector3.right;
            if (TargetInFieldOfView(target,sightAngle,sightDist) && TargetWithClearVision(target))
            {
                Fire();
                if (Vector3.Distance(target.position, this.transform.position) >= 3f)
                {

                    moveDirection = target.position - gameObject.transform.position;
                    Debug.Log(moveDirection);
                } else
                {
                    Vector3 vectorToTarget = target.position - transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

                    moveDirection = Vector3.zero;
                }
            }
            else
            {
                moveDirection = SearchWayPoint(moveDirection);
            }


            //Update speed based on the directional movement and the defined movement speed
            rig.velocity = moveDirection.normalized * speed;
        }
    }
}
