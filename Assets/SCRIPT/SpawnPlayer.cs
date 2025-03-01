using UnityEngine;
using Photon.Pun;
public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;


    private void Start()
    {
        Vector3 randPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ,maxZ));
        PhotonNetwork.Instantiate(playerPrefab.name, randPosition, Quaternion.identity);
    }

}
