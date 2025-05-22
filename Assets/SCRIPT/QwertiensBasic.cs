using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class QwertiensBasic : MonoBehaviourPunCallbacks, IPunObservable
{
    public NavMeshAgent agent2;
    public float stopDistance = 8F; // Distance minimale avant d'arrêter la poursuite

    private Vector3 lastTargetPosition;
    public Slider healthBar;
    public int attackDamage = 10;
    public float attackCooldown = 4f; 
    private float lastAttackTime;

    private GameObject currentTarget;

    private int Health = 100;
    private void Awake()
    {
        
    }
    void Start()
    {

        agent2 = GetComponent<NavMeshAgent>();
        agent2.speed = 3f;
        Debug.Log("sndv");
        InvokeRepeating(nameof(FindClosestPlayer), 0f, 1f); // Cherche une cible toutes les secondes
    }
    void Update()
    {
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distance > stopDistance)
            {
                // Poursuit le joueur
                agent2.isStopped = false;
                agent2.SetDestination(currentTarget.transform.position);
            }
            else
            {
                // Arrête de bouger
                agent2.isStopped = true;

                // Attaque si le cooldown est terminé
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }

    }

    void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float minDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject player in players)
        {
            // Ignore les joueurs morts ou désactivés
            if (!player.activeInHierarchy)
                continue;

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        currentTarget = closest;
    }
    private void AttackPlayer()
    {
        if (currentTarget != null)
        {
            IDamageable damageable = currentTarget.GetComponent<IDamageable>();
            PhotonView targetPhotonView = currentTarget.GetComponent<PhotonView>();

            if (damageable != null && targetPhotonView != null && targetPhotonView.IsMine)
            {
                if (damageable != null && targetPhotonView != null)
                {
                    targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, (float)attackDamage);
                }
                Debug.Log("💥 Qwertien attaque et inflige des degats !");
            }

            lastAttackTime = Time.time;
        }
    }

    [PunRPC]
    public void SetHealth(int i)
    {
        Health -= i;

        if (healthBar != null)
            healthBar.value = Health;

        if (Health <= 0)
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            Health = (int)stream.ReceiveNext();
            if (healthBar != null)
                healthBar.value = Health;
        }
    }

}
