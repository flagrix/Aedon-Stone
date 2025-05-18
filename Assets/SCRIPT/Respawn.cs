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
    [SerializeField] private GameObject Réticule;

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
        respawnannoncement.gameObject.SetActive(false); // Masquer l'annonce au d�part
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
        Réticule.gameObject.SetActive(false);
        respawnannoncement.gameObject.SetActive(true);
        TimeRespawnCount.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetweenRespawn);
        Réticule.gameObject.SetActive(true);
        respawnannoncement.gameObject.SetActive(false);
        TimeRespawnCount.gameObject.SetActive(false);
        Debug.Log("Respawn du joueur !");
        player.transform.position = player.respawnPoint; // Replace le joueur au point de respawn
        player.gameObject.SetActive(true); // R�active l'objet joueur
        player.enabled = true; // R�active le script de mouvement du joueur

        // Configure la cam�ra en mode premi�re personne
        Camera.main.transform.SetParent(player.transform); // Rattache la cam�ra au joueur
        Camera.main.transform.localPosition = new Vector3(0.33f, 0.7f, -0.151f); // Positionne la cam�ra au centre du joueur (premi�re personne)
        Camera.main.transform.localRotation = Quaternion.identity; // R�initialise la rotation de la cam�ra
        /**if (HealthBar.instance != null)
        {
            HealthBar.instance.SetActualHealth(100);
        }**/
        if (timeBetweenRespawn < 30f)
            timeBetweenRespawn += 5f;
        countdown = 0f;
        player.isRespawning = false;
    }
}
