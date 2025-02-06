using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private float timeBetweenRespawn = 10f;
    [SerializeField] private Text respawnannoncement;
    [SerializeField] private Text TimeRespawnCount;

    private float countdown = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //countdown = timeBetweenWave;
        respawnannoncement.gameObject.SetActive(false); // Masquer l'annonce au départ
        TimeRespawnCount.gameObject.SetActive(false);
    }

    void Update()
    {
        if (PlayerMovement.instance.isRespawning)
        {
            if (countdown <= 0f)
            {
                countdown = (int)(Mathf.Floor(timeBetweenRespawn + 1));
                
            }
            countdown -= Time.deltaTime;
            TimeRespawnCount.text = Mathf.Floor(countdown).ToString();
        }
    }

    public void StartRespawnCoroutine(PlayerMovement player)
    {
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(PlayerMovement player)
    {
        Debug.Log("Attente de 10 secondes avant le respawn...");
        respawnannoncement.gameObject.SetActive(true);
        TimeRespawnCount.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetweenRespawn);
        respawnannoncement.gameObject.SetActive(false);
        TimeRespawnCount.gameObject.SetActive(false);
        Debug.Log("Respawn du joueur !");
        player.transform.position = player.respawnPoint; // Replace le joueur au point de respawn
        player.gameObject.SetActive(true); // Réactive l'objet joueur
        player.enabled = true; // Réactive le script de mouvement du joueur

        // Configure la caméra en mode première personne
        Camera.main.transform.SetParent(player.transform); // Rattache la caméra au joueur
        Camera.main.transform.localPosition = Vector3.zero; // Positionne la caméra au centre du joueur (première personne)
        Camera.main.transform.localRotation = Quaternion.identity; // Réinitialise la rotation de la caméra
        HealthBar.instance.SetActualHealth(100);
        if (timeBetweenRespawn < 30f)
            timeBetweenRespawn += 5f;
        countdown = 0f;
        player.isRespawning = false;
    }
}
