using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;
using System.IO;
public class PlayerManager : MonoBehaviour
{
    PhotonView Hp;

    private void Awake()
    {
        Hp = GetComponent<PhotonView>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Hp.IsMine)
        {
            CreateController();
        }
    }

    // Update is called once per frame
    void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero,Quaternion.identity);
    }
}
