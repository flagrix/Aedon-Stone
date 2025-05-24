using UnityEngine;
using Photon.Pun;

public class spawnouvrier : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] private PhotonView PV;
    public GameObject[] SpawnPoints;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        PV.RPC("RPC_Spawn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_Spawn()
    {
        foreach (var spawn in SpawnPoints)
        {
            Instantiate(prefab, spawn.transform.position, Quaternion.Euler(0, -90, 0));
        }
        
    }
}
