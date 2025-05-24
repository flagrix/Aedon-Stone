using UnityEngine;
using Photon.Pun;

public class spawnouvrier : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public GameObject[] SpawnPoints;

    void Start()
    {
        foreach (var spawn in SpawnPoints)
        {
            Instantiate(prefab, spawn.transform.position, Quaternion.Euler(0, -90, 0));
        }
    }
    
}
