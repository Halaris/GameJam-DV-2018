using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{

    public int damage;
    public CharacterBaseController parent;
    [SerializeField] private float creationTime;
    [SerializeField] private float activationTime = 6f;
    [SerializeField] private LayerMask characters;
    [SerializeField] private LayerMask obstacle;
    [SerializeField] private GameObject explosion;
	[SerializeField] private AudioSource explosionAS;
	[SerializeField] private AudioSource deployAS;
    public float explosionRadius = 3f;
    // Use this for initialization

    void Start()
    {
        creationTime = Time.time;
		Destroy(Instantiate(deployAS, transform.position, transform.rotation),1);
    }

    bool TargetWithClearVision(Transform targetPoint, LayerMask maskToAvoid)
    {
        Vector3 dirToTarget = (targetPoint.position - transform.position).normalized;
        float dist = Vector3.Distance(targetPoint.position, this.transform.position);
        return !Physics2D.Raycast(transform.position, dirToTarget, dist, maskToAvoid);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] charactersInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius, characters);
        foreach ( Collider2D character in charactersInRange)
        {
            if(TargetWithClearVision(character.transform, obstacle))
            {
                character.gameObject.GetComponent<CharacterBaseController>().GetDamaged(damage);
            }
        }

		parent.minesLeft++;
        GameObject explosionObj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		Destroy(Instantiate(explosionAS, transform.position, transform.rotation),1);
		Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (creationTime + activationTime <= Time.time)
        {
            GetComponent<Animator>().SetTrigger("Active");
            GetComponent<Collider2D>().enabled = true;
        }
    }
}
