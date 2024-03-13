using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject[] EnemyType;
    public GameObject spawnVX;


    bool spawnAvailable = true;
    int i = 0;

    private void Start()
    {
        GameManager.instance.enemyCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnAvailable && GameManager.instance.enemyCount < 11 && !GameManager.IsGameOver())
        {
            
            StartCoroutine(SpawnOne());
        }

    }

    IEnumerator SpawnOne()
    {
        spawnAvailable = false;
        
        i = Random.Range(0, 3);

        Instantiate(spawnVX, transform.position, Quaternion.identity);
        Instantiate(EnemyType[i], transform.position, Quaternion.identity);

        GameManager.instance.enemyCount++;

        yield return new WaitForSeconds(4f);

        spawnAvailable = true;
    }
}
