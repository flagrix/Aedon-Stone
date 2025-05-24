using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class WavespawnerGauche : MonoBehaviour
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
            yield return new WaitForSeconds(1f);
            if (i > 0)
            {
                if (i % 2 == 0)
                {
                    SpawnEnemyfat();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyfat();
                    yield return new WaitForSeconds(1f);
                }
                if (i % 4 == 0)
                {
                    SpawnEnemyfast();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemyfast();
                    yield return new WaitForSeconds(1f);
                }
                if (i % 5 == 0)
                {
                    SpawnEnemyfat();
                    yield return new WaitForSeconds(1f);
                    SpawnEnemy();
                    yield return new WaitForSeconds(1f);

                }
                yield return new WaitForSeconds(1.5f);
            }
        }
        

    }

    void SpawnEnemy()
    {
        GameObject enemyObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "qwertien_gauche"), spawnPoint.position, spawnPoint.rotation, 0);
        ennemy e = enemyObj.GetComponent<ennemy>();
        e.FinalTakeDamage(-20 * (waveIndex - 1));
    }
    void SpawnEnemyfast()
    {
        GameObject enemyObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FastQwert_g"), spawnPoint.position, spawnPoint.rotation, 0);
        ennemy e = enemyObj.GetComponent<ennemy>();
        e.FinalTakeDamage(-20 * (waveIndex - 1));
    }
    void SpawnEnemyfat()
    {
        GameObject enemyObj =PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "FatQwert_g"), spawnPoint.position, spawnPoint.rotation, 0);
        ennemy e = enemyObj.GetComponent<ennemy>();
        e.FinalTakeDamage(-20 * (waveIndex - 1));
    }
}
