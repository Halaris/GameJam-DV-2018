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
    public float explosionRadius = 3f;
    // Use this for initialization
    void Start()
    {
        creationTime = Time.time;

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

		if (gameObject.GetComponent<AudioSource>() != null) {
			gameObject.GetComponent<AudioSource>().Play();
		}

        parent.minesLeft++;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (creationTime + activationTime <= Time.time)
            GetComponent<Collider2D>().enabled = true;
    }
}
