using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    [SerializeField] protected GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 3F;
    [SerializeField] private float nextSpawn = 0.0F;
    [SerializeField] private int maxQuantityEnemy = 5;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask characters;
    [SerializeField] private float characterMinConflictRange = 3F;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextSpawn)
        {
            if (transform.childCount < maxQuantityEnemy)
            {
                int len = Physics2D.OverlapCircleAll(transform.position, characterMinConflictRange, characters).Length;
                Debug.Log(len);
                if (len == 0) {
                    GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
                    var enemyController = newEnemy.GetComponent<EnemyController>();
                    newEnemy.transform.SetParent(transform);
                    enemyController.target = target;
                    nextSpawn = Time.time + spawnRate;
                }
            }
        }
    }
}
