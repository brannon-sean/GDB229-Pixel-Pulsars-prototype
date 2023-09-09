using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemySpawnList;
    [SerializeField] List<GameObject> mediumEnemySpawnList;
    [SerializeField] List<GameObject> bossEnemySpawnList;

    [SerializeField] float spawnRadiusMax;
    [SerializeField] float spawnRadiusMin;
    [SerializeField] int waveSize;

    [SerializeField] int enemySpawnCount;
    [SerializeField] int mediumEnemySpawnCount;
    [SerializeField] int bossEnemySpawnCount;

    private int previousEnemySpawnCount;
    private int previousMediumSpawnCount;
    private int previousBossSpawnCount;
    private int previousWaveSize;

    [SerializeField] int timeToCompleteWave;
    private int wave;
    private bool waveSpawned;

    private void Update()
    {
        if (!waveSpawned)
        {
            StartCoroutine(waveSpawner());
        }
    }

    private void spawnWave()
    {
        previousEnemySpawnCount = enemySpawnCount;
        previousMediumSpawnCount = mediumEnemySpawnCount;
        previousBossSpawnCount = bossEnemySpawnCount;
        previousWaveSize = waveSize;
        for (int i = 0; i < waveSize; i++)
        {
            Vector3 spawnPosition = getRandomSpawnPosition();
            GameObject enemyToSpawn = getRandomEnemy();

            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 getRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(spawnRadiusMin, spawnRadiusMax);
        Vector3 spawnPosition = new Vector3(randomCircle.x, 0f, randomCircle.y);

        //Fix for issue with enemies spawning in the air and not being able to drop down to the navmesh.
        NavMeshHit hit;
        if (NavMesh.SamplePosition(gamemanager.instance.player.transform.position + spawnPosition, out hit, 100f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return gamemanager.instance.player.transform.position + spawnPosition;
        }
    }

    private GameObject getRandomEnemy()
    {
        float randomValue = Random.value;

        if(randomValue < (enemySpawnCount / (float)waveSize))
        {
            enemySpawnCount--;
            return enemySpawnList[Random.Range(0, enemySpawnList.Count)];
        }else if(randomValue < ((enemySpawnCount + mediumEnemySpawnCount) / (float)waveSize))
        {
            mediumEnemySpawnCount--;
            return mediumEnemySpawnList[Random.Range(0, mediumEnemySpawnList.Count)];
        }
        else
        {
            bossEnemySpawnCount--;
            return bossEnemySpawnList[Random.Range(0, bossEnemySpawnList.Count)];
        }
    }
    private IEnumerator waveSpawner()
    {
        spawnWave();
        waveSpawned = true;
        yield return new WaitForSeconds(timeToCompleteWave);
        float scaler = Random.Range(1.01f, 1.45f);
        enemySpawnCount = (int)(previousEnemySpawnCount * scaler);
        mediumEnemySpawnCount = (int)(previousMediumSpawnCount * scaler);
        bossEnemySpawnCount = (int)(previousBossSpawnCount * scaler);
        waveSize = (int)(previousWaveSize * scaler);
        wave++;
        waveSpawned = false;
    }
}
