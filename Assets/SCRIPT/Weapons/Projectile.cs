using UnityEngine;

using UnityEngine;

using Photon.Pun;

public class Projectile : MonoBehaviourPun
{
    private Vector3 target;
    private float speed;
    private bool initialized = false;
    private float destroyDistance = 0.1f;

    public void Initialize(Vector3 targetPoint, float moveSpeed)
    {
        target = targetPoint;
        speed = moveSpeed;
        initialized = true;
    }

    void Update()
    {
        if (!photonView.IsMine) return; // 👈 Seul le propriétaire déplace l'objet
        Debug.Log("Projectile Update");
        if (!initialized) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < destroyDistance)
        {
            PhotonNetwork.Destroy(gameObject); // 🧨 Détruit sur tous les clients
        }
    }
}

