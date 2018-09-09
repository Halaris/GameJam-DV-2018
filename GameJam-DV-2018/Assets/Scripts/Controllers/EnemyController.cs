using System;
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
    [SerializeField] protected GameObject lifePrefab;
    [SerializeField] protected static float chanceToSpawnLife = 0.0001F;
    [SerializeField] private Transform lastVisitedPoint;
    [SerializeField] private Queue<Transform> visitedPoints = new Queue<Transform>();
    [SerializeField] private string wayPointLayer;
    [SerializeField] private GameObject explosion;

    bool TargetInFieldOfView(Transform targetPoint, float viewAngle, float viewDist)
    {
        Vector3 targetDir = targetPoint.position - transform.position;
        float enemyAngle = Vector3.Angle(targetDir, lastDirection);
        var dist = Vector3.Distance(targetPoint.position, this.transform.position);
        return (enemyAngle < viewAngle && dist < viewDist);
    }
    bool TargetWithClearVision(Transform targetPoint, LayerMask maskToAvoid)
    {
        Vector3 dirToTarget = (targetPoint.position - transform.position).normalized;
        float dist = Vector3.Distance(targetPoint.position, this.transform.position);
        return !Physics2D.Raycast(transform.position, dirToTarget, dist, maskToAvoid);
    }

    protected override void Die()
    {
        spawnLife();
        target.GetComponent<PlayerController>().IncreaseScore(100);
        GameObject explosionObj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Destroy(gameObject, 0);
    }
    private void spawnLife()
    {
        if (GameObject.FindGameObjectsWithTag("Life").Length == 0 && target.GetComponent<PlayerController>().lives < 3)
        {
            if (UnityEngine.Random.value <= chanceToSpawnLife)
            {
                GameObject life = Instantiate(lifePrefab, transform.position, transform.rotation) as GameObject;
                resetChance();
            }
            else
            {
                chanceToSpawnLife *= 2;
            }
        }

    }
    private void resetChance()
    {
        chanceToSpawnLife = 0.0001F;
    }

    protected override void LoseLife()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            transform.Rotate(Vector3.forward * 180);
            visitedPoints.Clear();
            lastVisitedPoint = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (LayerMask.LayerToName(collision.gameObject.layer) == wayPointLayer)
        {
            if (lastVisitedPoint != null)
            {
                visitedPoints.Enqueue(lastVisitedPoint);
            }
            lastVisitedPoint = collision.transform;
        } 
    }
    
    private Vector3 SearchWayPoint(Vector3 moveDirection, float pointDistanceDetection, float pointAngleDetection)
    {
        //moveDirection = gameObject.transform.position - origPos
        SortedDictionary<double, Transform> visiblePoints = new SortedDictionary<double, Transform>();
        Collider2D[] wayPointsInViewRadius = Physics2D.OverlapCircleAll(transform.position, pointDistanceDetection, wayPoints);
        foreach (Collider2D wayPointInFieldOfView in wayPointsInViewRadius)
        {
            Transform targetPoint = wayPointInFieldOfView.transform;
            if (targetPoint != lastVisitedPoint && TargetInFieldOfView(targetPoint, pointAngleDetection, pointDistanceDetection) && TargetWithClearVision(targetPoint,wall) && TargetWithClearVision(targetPoint, this.gameObject.layer))
            {
                
                float dist = Vector3.Distance(targetPoint.position, transform.position);
             
                    float priorityLose;
                    priorityLose = targetPoint.gameObject.GetComponent<WayPointController>().priority;
                    float priorityValue = dist / (priorityLose);
                    if (!visitedPoints.Contains(targetPoint))
                        visiblePoints.Add(priorityValue, targetPoint);
                
            }
        }
        SortedDictionary<double, Transform>.Enumerator numerator = visiblePoints.GetEnumerator();
        if (numerator.MoveNext())
        {
            return numerator.Current.Value.position - transform.position;
        } else if(visitedPoints.Count >0)
        {
            return visitedPoints.Dequeue().position - transform.position;
        }
        return moveDirection;
    }
    protected override void CharacterUpdate()
    {
        if (target != null)
        {
            Vector3 moveDirection = Vector3.zero;
            if (TargetInFieldOfView(target,sightAngle,sightDist) && TargetWithClearVision(target, wall))
            {
                if (Vector3.Distance(target.position, this.transform.position) >= 3f)
                {

                    moveDirection = target.position - gameObject.transform.position;
                } else
                {
                    Vector3 vectorToTarget = target.position - transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

                    moveDirection = Vector3.zero;
                }
                if(TargetInFieldOfView(target, sightAngle/3, sightDist))
                {
                    Fire();
                }
            }
            else
            {
                moveDirection = SearchWayPoint(moveDirection, sightDist*2, sightAngle*1.5f);
            }


            //Update speed based on the directional movement and the defined movement speed
            rig.velocity = moveDirection.normalized * speed;
        }
    }
}
