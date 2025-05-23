using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class WaveSpawner : MonoBehaviour
{

    [SerializeField]
    private Transform enemyPrefab;


    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float timeBetweenWaves = 30f;

    private float countdown = 2f;

    private int waveIndex = 0;



    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            SpawnEnemy();
            if (i % 2 == 0)
            {
                SpawnEnemyfat();
                SpawnEnemy();
            }
            if (i % 4 == 0)
            {
                SpawnEnemyfast();
                SpawnEnemy();
            }
            if (i % 5 == 0)
            {
                SpawnEnemyfat();
                SpawnEnemy();
                SpawnEnemy();
                SpawnEnemy();
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    void SpawnEnemy()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "qwertien"), spawnPoint.position, spawnPoint.rotation, 0);
    }
    void SpawnEnemyfast()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FastQwert"), spawnPoint.position, spawnPoint.rotation, 0);
    }
    void SpawnEnemyfat()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FatQwert"), spawnPoint.position,spawnPoint.rotation, 0);
    }
}