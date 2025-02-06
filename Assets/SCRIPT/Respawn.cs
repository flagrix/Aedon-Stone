using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    public void StartRespawnCoroutine(PlayerMovement player)
    {
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(PlayerMovement player)
    {
    
        Debug.Log("Attente de 10 secondes avant le respawn...");
        yield return new WaitForSeconds(10f);

        Debug.Log("Respawn du joueur !");
        player.transform.position = player.respawnPoint; // Replace le joueur au point de respawn
        player.gameObject.SetActive(true); // R�active l'objet joueur
        player.enabled = true; // R�active le script de mouvement du joueur

        // Configure la cam�ra en mode premi�re personne
        Camera.main.transform.SetParent(player.transform); // Rattache la cam�ra au joueur
        Camera.main.transform.localPosition = Vector3.zero; // Positionne la cam�ra au centre du joueur (premi�re personne)
        Camera.main.transform.localRotation = Quaternion.identity; // R�initialise la rotation de la cam�ra
        HealthBar.instance.SetActualHealth(100);
        player.isRespawning = false;
    }
}
