using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    PhotonView Hp;
    GameObject controller;

    [SerializeField] private float timeBetweenRespawn = 10f; // pour le respawn
    [SerializeField] private Text respawnannoncement;
    [SerializeField] private Text TimeRespawnCount;
    [SerializeField] public GameObject Reticule;
    private bool isRespawning = false;
    private float countdown = 0f;
    private float respawnStartTime;

    public bool isGameOver = false;

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
                SetRespawnUI(false);
        }
    }

    void Update()
    {
        if (isRespawning && Hp.IsMine)
        {
            UpdateCountdown();
        }
        if (Hp.IsMine && isGameOver && Reticule.activeSelf)
        {
            Reticule.SetActive(false);
        }
    }

    // Update is called once per frame
    void CreateController()
    {
        Transform spawnpoint = SpawnManager.instance.GetSpawnPoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "HumanPlayer"), spawnpoint.position,spawnpoint.rotation, 0, new object[]{Hp.ViewID});
    }

    public void StartRespawn()
    {
        if (!isRespawning && Hp.IsMine)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isRespawning = true;
        respawnStartTime = Time.time;
        SetRespawnUI(true);

        // Désactiver proprement
        if (controller != null)
        {
            var pc = controller.GetComponent<PlayerController>();
            if (pc != null) pc.SetActive(false);

            PhotonNetwork.Destroy(controller);
            controller = null;
        }

        // Attendre le délai complet
        yield return new WaitForSeconds(timeBetweenRespawn);

        // Recréation
        CreateController();
        SetRespawnUI(false);
        isRespawning = false;
    }

    private void UpdateCountdown()
    {
        if (respawnannoncement == null || TimeRespawnCount == null) return;

        float remainingTime = Mathf.Max(0f, timeBetweenRespawn - (Time.time - respawnStartTime));
        TimeRespawnCount.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    private void SetRespawnUI(bool active)
    {
        if (respawnannoncement != null) respawnannoncement.gameObject.SetActive(active);
        if (TimeRespawnCount != null) TimeRespawnCount.gameObject.SetActive(active);
        if (Reticule != null) Reticule.SetActive(!active);
    }
}
