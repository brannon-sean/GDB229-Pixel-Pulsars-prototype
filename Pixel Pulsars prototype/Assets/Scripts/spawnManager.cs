using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class spawnManager : MonoBehaviour
{
    [Header("--- Enemy Components ---")]
    //Normal enemies
    [SerializeField] List<GameObject> enemySpawnList;
    //Medium enemies
    [SerializeField] List<GameObject> mediumEnemySpawnList;
    //Boss enemies
    [SerializeField] List<GameObject> bossEnemySpawnList;

    [Header("--- Spawning Characteristics ---")]
    [SerializeField] float spawnRadiusMax;
    [SerializeField] float spawnRadiusMin;
    [SerializeField] int waveSize;
    [SerializeField] float scaleMin;
    [SerializeField] float scaleMax;
    [SerializeField] int enemySpawnCount;
    [SerializeField] int mediumEnemySpawnCount;
    [SerializeField] int bossEnemySpawnCount;
    [SerializeField] int timeToCompleteWave;
    [SerializeField] int timeToNextWave;
    private float timeLeftinWave;
    private int waveNumber = 1;

    //Variable definitions:
    private int previousEnemySpawnCount;
    private int previousMediumSpawnCount;
    private int previousBossSpawnCount;
    private int previousWaveSize;
    private int wave;
    private bool waveSpawned;
    private bool triggerEntered;

    private void Update()
    {
        if (!waveSpawned && triggerEntered)
        {

            StartCoroutine(waveSpawner());
            StartCoroutine(WaveTimer());

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

        float scaler = Random.Range(scaleMin, scaleMax);
        enemySpawnCount = (int)(previousEnemySpawnCount * scaler);
        mediumEnemySpawnCount = (int)(previousMediumSpawnCount * scaler);
        bossEnemySpawnCount = (int)(previousBossSpawnCount * scaler);
        waveSize = (int)(previousWaveSize * scaler);
        wave++;
        waveNumber++;
        gamemanager.instance.waveNumberText.text = waveNumber.ToString("0");
        waveSpawned = false;
    }

    IEnumerator WaveTimer()
    {
        timeLeftinWave = timeToCompleteWave;
        while (timeLeftinWave > 0)
        {
            gamemanager.instance.waveTimerText.text = timeLeftinWave.ToString("0");
            timeLeftinWave -= Time.deltaTime;
            yield return null;
        }
    }

    public void startSpawning()
    {
        triggerEntered = true;
    }

}
