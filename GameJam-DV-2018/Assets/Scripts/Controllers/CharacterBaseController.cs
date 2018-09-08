using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterBaseController : MonoBehaviour {

    public Text scoreValueText;

    [SerializeField] protected float speed;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected long score;
    [SerializeField] protected long currentLifeScore;
    [SerializeField] protected int lives;
    [SerializeField] protected string enemyTag;
    [SerializeField] protected int damage;
    protected Rigidbody2D rig;
    protected Vector3 origPos;
    protected Vector3 lastDirection = Vector3.right;
    [SerializeField] private float fireRate = 0.5F;
    [SerializeField]  private float nextFire = 0.0F;


    private void Awake()
    {
        rig = this.GetComponent<Rigidbody2D>();
        //Set original position
        origPos = gameObject.transform.position;

    }
    public void GetDamaged(int damage)
    {
        Debug.Log("Fue dañado");
        this.lives -= damage;
        this.LoseLife();
        if (lives <= 0)
        {
            Die();
        }
    }

    protected void Fire()
    {
		if(Time.time > nextFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + lastDirection, transform.rotation) as GameObject;
			projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(lastDirection.x, lastDirection.y) * projectileSpeed;
			var bulletController = projectile.GetComponent<BulletController>();
			bulletController.enemyTag = enemyTag;
			bulletController.damage = damage;
			bulletController.sourceTag = gameObject.tag;
			projectile.gameObject.tag = gameObject.tag + "bullet";
			nextFire = Time.time + fireRate;
        }

    }

    private void Rotate(Vector3 moveDirection)
    { 
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private Vector3 UpdateDirection()
    {
        //Obtain direction vector
        Vector3 moveDirection = gameObject.transform.position - origPos;


        if (!V3Equal(moveDirection, Vector3.zero))
        {
            lastDirection = moveDirection.normalized;
        }

        //Update original position
        origPos = gameObject.transform.position;
        return moveDirection;
    }
    // Update is called once per frame
    protected void UpdateRotation()
    {
        Rotate(UpdateDirection());
    }

    private void Update()
    {
        UpdateRotation();
        CharacterUpdate();
    }

	protected bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.00000001;
    }

    public void IncreaseScore(long points)
    {
        score += points;
        currentLifeScore += points;
        scoreValueText.text = score.ToString();
    }

    protected abstract void CharacterUpdate();
    protected abstract void LoseLife();
    protected abstract void Die();
}
