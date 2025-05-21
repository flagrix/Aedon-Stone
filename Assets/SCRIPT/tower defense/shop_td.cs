using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class shop_td : MonoBehaviour
{

    public Vector3 positionOffset;

    private BuildManager buildManager;

    public void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void PurchaseStandardTurret()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "canon_vrai 1"), transform.position + positionOffset, transform.rotation, 0);
    }
    

        
}
