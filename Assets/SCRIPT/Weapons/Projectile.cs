using UnityEngine;

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float maxLifetime = 5f;
    public GameObject impactEffect; // prefab d'explosion

    private Vector3 targetPoint;

    public void SetTarget(Vector3 target)
    {
        targetPoint = target;
        Destroy(gameObject, maxLifetime); // auto-détruire après X secondes si rien touché
    }

    void Update()
    {
        // Mouvement vers la cible
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        // Détecte l’arrivée
        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            Instantiate(impactEffect, transform.position, Quaternion.Euler(90f, 0, 0));
        }
    }
    
}
