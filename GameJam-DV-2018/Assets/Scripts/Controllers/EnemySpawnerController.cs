using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    [SerializeField] protected Transform enemyGroup;
    [SerializeField] protected GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 0.5F;
    [SerializeField] private float nextSpawn = 0.0F;
    [SerializeField] private int maxQuantityEnemy = 5;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextSpawn)
        {
            if (enemyGroup.childCount < maxQuantityEnemy)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
                var bulletController = newEnemy.GetComponent<EnemyController>();
                newEnemy.transform.SetParent(enemyGroup);
                /*bulletController.enemyTag = enemyTag;
                bulletController.damage = damage;
                bulletController.sourceTag = gameObject.tag;
                newEnemy.gameObject.tag = gameObject.tag + "bullet";*/
            }
            nextSpawn = Time.time + spawnRate;
        }
    }
}
