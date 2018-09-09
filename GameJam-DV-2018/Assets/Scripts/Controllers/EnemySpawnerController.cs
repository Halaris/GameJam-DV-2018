using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> original;

        public ReverseComparer(IComparer<T> original)
        {
            // TODO: Validation
            this.original = original;
        }

        public int Compare(T left, T right)
        {
            return original.Compare(right, left);
        }
    }

    [SerializeField] protected GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 3F;
    [SerializeField] private float nextSpawn = 0.0F;
    [SerializeField] public int maxQuantityEnemy = 5;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask characters;
    public SortedDictionary<float,Transform> pathDictionary = new SortedDictionary<float, Transform>(new ReverseComparer<float>(Comparer<float>.Default));
    Stack<Transform> pathStack = new Stack<Transform>();
    [SerializeField] private float characterMinConflictRange = 3F;
    protected Transform path;
    protected Transform batallion;
    // Use this for initialization
    private void Awake()
    {
        for( int x = 0; x< transform.childCount; x++)
        {
            if(transform.GetChild(x).tag == "Path")
                path = transform.GetChild(x);
            if (transform.GetChild(x).tag == "Batallion")
                batallion = transform.GetChild(x);
        }
        foreach (Transform pathNode in path)
            pathDictionary.Add(pathNode.GetComponent<WayPointController>().priority,pathNode);
        
    }
    // Update is called once per frame
    void Update () {
        if (Time.time > nextSpawn)
        {
            if (batallion.childCount < maxQuantityEnemy)
            {
                int len = Physics2D.OverlapCircleAll(transform.position, characterMinConflictRange, characters).Length;
                if (len == 0) {
                    pathStack = new Stack<Transform>();
                    foreach (KeyValuePair<float, Transform> entry in pathDictionary)
                        pathStack.Push(entry.Value);
                    GameObject newEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
                    var enemyController = newEnemy.GetComponent<EnemyController>();
                    enemyController.path = pathStack;
                    newEnemy.transform.SetParent(batallion);
                    enemyController.target = target;
                    nextSpawn = Time.time + spawnRate;
                }
            }
        }
    }
}
