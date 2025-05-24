using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class SpawnerIA : MonoBehaviour
{

    [SerializeField]
    private Transform enemyPrefab;


    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float timeBetweenWaves = 45f;

    private float countdown = 60f;

    private int waveIndex = 0;



    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

    }

    IEnumerator SpawnWave()
    {
        if (waveIndex < 3)
            waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1.5f);
            if (i % 2 == 0)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1.5f);
            }
        }

    }

    void SpawnEnemy()
    {
        GameObject enemyObj =PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BasicQwertien"), spawnPoint.position, spawnPoint.rotation, 0);
        ennemy e = enemyObj.GetComponent<ennemy>();
        e.FinalTakeDamage(-20 * (waveIndex - 1));
    }
}