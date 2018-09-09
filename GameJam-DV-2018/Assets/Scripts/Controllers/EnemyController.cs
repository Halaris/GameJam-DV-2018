using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBaseController
{
    private class Target
    {
        public Target(Vector3 position)
        {
            this.position = position;
        }
        public Vector3 position;
    }

    public float sightAngle = 120f;
    public float sightDist = 100f;
    public LayerMask wall;
    public LayerMask wayPoints;
    public Transform target;
    [SerializeField] protected GameObject lifePrefab;
    [SerializeField] protected static float chanceToSpawnLife = 0.001F;
    [SerializeField] private Stack<Transform> visitedPoints = new Stack<Transform>();
    [SerializeField] private string wayPointLayer;
    [SerializeField] private GameObject explosion;
    public Stack<Transform> path;
	[SerializeField] private AudioSource audioSource;
    [SerializeField] private Target currentTarget;
    private Vector3 lastPoint;
    private Action currentAction = Action.followingPath;
    enum Action { followingPath, returningPath, followingPlayer, returningFromPersecution, idle };
    
    /// <summary>
    /// Makes a choice to where to move
    /// </summary>
    void move()
    {
        //First, review if it already get to its current target
        if (currentTarget != null && Vector2.Distance(currentTarget.position, transform.position) < 1.5f)
        {
            currentTarget = null;
        }
        //Search player and try to attack it
        if (searchPlayer())
        {
            currentAction = Action.followingPath;
        }
        else if (currentTarget == null) //If there is no player on view and it have no current target, it execute it,s corresponding action
        {
            switch (currentAction)
            {
                //Ensure that it go to the last point the user was seen
                case Action.followingPlayer:
                    if (lastPoint == null)
                    {
                        lastPoint = transform.position;
                    }
                    if (currentTarget == null)
                    {
                        // If it gets to the last seen location of the player,it begins  to go to the last location he have when he had seen the user
                        currentTarget = new Target(lastPoint);
                        currentAction = Action.returningFromPersecution;
                    }
                    break;
                case Action.returningFromPersecution:
                    if (currentTarget == null)
                    {
                        if (isVisible(path.Peek().position, sightAngle, sightDist))
                        {
                            currentTarget = new Target(path.Peek().position);
                            currentAction = Action.followingPath;
                        }
                        else if (isVisible(visitedPoints.Peek().position, sightAngle, sightDist))
                        {
                            currentTarget = new Target(visitedPoints.Peek().position);
                            currentAction = Action.followingPath;
                        }
                        else
                        {
                            currentAction = Action.idle;
                        }
                        currentAction = Action.returningFromPersecution;
                    }
                    break;
                case Action.followingPath:
                    if (currentTarget == null)
                    {
                        if (path.Count > 0)
                        {
                            currentTarget = new Target(path.Peek().position);
                            SwitchStacks(path, visitedPoints);
                        }
                        else
                        {
                            currentAction = Action.returningPath;
                        }
                    }
                    break;
                case Action.returningPath:
                    if (currentTarget == null)
                    {
                        if (visitedPoints.Count > 0)
                        {
                            currentTarget = new Target(visitedPoints.Peek().position);
                            SwitchStacks(visitedPoints, path);
                        }
                        else
                        {
                            currentAction = Action.followingPath;
                        }
                    }

                    break;
                case Action.idle:
                    Quaternion q = Quaternion.AngleAxis(UnityEngine.Random.RandomRange(0, 360), Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

                    break;
            }
        }
    }

    void SwitchStacks(Stack<Transform> one, Stack<Transform> two)
    {
        two.Push(one.Pop());
    }

    bool isVisible(Vector3 targetPoint, float viewAngle, float viewDist)
    {
        return TargetInFieldOfView(targetPoint, viewAngle, viewDist) && TargetWithClearVision(targetPoint, wall);
    }

    bool searchPlayer()
    {
        Vector3 moveDirection = Vector3.zero;
        if (TargetInFieldOfView(target.position, sightAngle, sightDist) && TargetWithClearVision(target.position, wall))
        {
            if (Vector3.Distance(target.position, this.transform.position) >= 3f)
            {

                currentTarget = new Target(target.position);

            }
            else
            {
                Vector3 vectorToTarget = target.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

                currentTarget = null;
            }
            CanShoot();
            return true;
        }
        return false;
    }

    void CanShoot()
    {
        if (TargetInFieldOfView(target.position, sightAngle / 3, sightDist))
        {
            Fire();
        }
    }

    Vector3 UpdateDirection()
    {
        return currentTarget.position - gameObject.transform.position;
    }

    bool TargetInFieldOfView(Vector3 targetPoint, float viewAngle, float viewDist)
    {
        Vector3 targetDir = targetPoint - transform.position;
        float enemyAngle = Vector3.Angle(targetDir, lastDirection);
        var dist = Vector3.Distance(targetPoint, this.transform.position);
        return (enemyAngle < viewAngle && dist < viewDist);
    }

    bool TargetWithClearVision(Vector3 targetPoint, LayerMask maskToAvoid)
    {
        Vector3 dirToTarget = (targetPoint - transform.position).normalized;
        float dist = Vector3.Distance(targetPoint, this.transform.position);
        return !Physics2D.Raycast(transform.position, dirToTarget, dist, maskToAvoid);
    }

    protected override void Die()
    {
        spawnLife();
        target.GetComponent<PlayerController>().IncreaseScore(100);
        GameObject explosionObj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		Instantiate(audioSource, transform.position, transform.rotation);
        Destroy(gameObject);
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
        }
    }


    protected override void CharacterUpdate()
    {
        if (target != null)
        {
            move();
            if (currentTarget != null)
            {
                rig.velocity = UpdateDirection().normalized * speed;
            }
        }


    }
}
