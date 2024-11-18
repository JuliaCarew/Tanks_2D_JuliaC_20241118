using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public int numberOfEnemiesToSpawn = 10;
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-11, 11), Random.Range(4, -7));
            // !!! CAUSES CRASH !!! //
            //Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
        }
    }
}
