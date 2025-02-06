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
        player.gameObject.SetActive(true); // Réactive l'objet joueur
        player.enabled = true; // Réactive le script de mouvement du joueur

        // Configure la caméra en mode première personne
        Camera.main.transform.SetParent(player.transform); // Rattache la caméra au joueur
        Camera.main.transform.localPosition = Vector3.zero; // Positionne la caméra au centre du joueur (première personne)
        Camera.main.transform.localRotation = Quaternion.identity; // Réinitialise la rotation de la caméra
        HealthBar.instance.SetActualHealth(100);
        player.isRespawning = false;
    }
}
