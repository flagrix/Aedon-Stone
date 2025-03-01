using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class QwertiensBasic : MonoBehaviour
{
    public NavMeshAgent agent2;
    public float stopDistance = 8F; // Distance minimale avant d'arrêter la poursuite

    private Vector3 lastTargetPosition;
    public Slider healthBar;
    public int attackDamage = 10;
    public float attackCooldown = 4f; 
    private float lastAttackTime;
    public static QwertiensBasic instance;

    private int Health = 100;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (PlayerMovement.instance != null)
        {
            // Récupérer la position actuelle du joueur
            Vector3 targetPos = PlayerMovement.instance.GetPlayerPosition();

            // Vérifier la distance entre l'agent et le joueur
            float distanceToPlayer = Vector3.Distance(agent2.transform.position, targetPos);

            // Si la distance est supérieure à la distance minimale ou que la cible a changé, recalculer le chemin
            if (distanceToPlayer > stopDistance)
            {
                if (targetPos != lastTargetPosition)
                {
                    agent2.SetDestination(targetPos);
                    lastTargetPosition = targetPos;
                }
            }
            else
            {
                // Arrêter le mouvement si l'agent est dans la portée minimale
                agent2.ResetPath();
                if (!GameOver.instance.isGameOver)
                    AttackPlayer();
            }
        }
    }

    public void SetHealth(int i)
    {
        Health += i;
        if (Health <= 0)
        {
            Destroy(gameObject);
            Inventory.instance.addRune(2);
        }
        if (healthBar != null)
        {
            healthBar.value = Health;
        }
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (HealthBar.instance != null)
            {
                HealthBar.instance.SetActualHealth(-attackDamage);
                Debug.Log("Qwertien attaque le joueur !");
            }

            // Mettre à jour le temps de la dernière attaque
            lastAttackTime = Time.time;
        }
    }

}
