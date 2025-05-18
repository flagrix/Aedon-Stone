using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FastQwertien : MonoBehaviour
{
    public NavMeshAgent agent2;
    public float stopDistance = 5F; // Distance minimale avant d'arrêter la poursuite


    public Slider healthBar;
    public int attackDamage = 10;
    public float attackCooldown = 4f;
    private float lastAttackTime;
    public static FastQwertien instance;

    private int Health = 80;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (PlayerMovement.instance != null)
        {

            Vector3 targetPos = new Vector3(414.6f, 2f, 626.3f);

            float distanceToPlayer = Vector3.Distance(agent2.transform.position, targetPos);

            // Si la distance est supérieure à la distance minimale ou que la cible a changé, recalculer le chemin
            if (distanceToPlayer > stopDistance)
            {
               agent2.SetDestination(targetPos); 
            }
            else
            {
                // Arrêter le mouvement si l'agent est dans la portée minimale
                agent2.ResetPath();
                if (!GameOver.instance.isGameOver)
                    AttackTower();
            }
        }
    }

    public void SetHealth(int i)
    {
        Health += i;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        if (healthBar != null)
        {
            healthBar.value = Health;
        }
    }

    private void AttackTower()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            if (TowerHealth.instance != null)
            {
                TowerHealth.instance.SetHealth(-attackDamage);
                Debug.Log("Qwertien attaque la tour !");
            }

            // Mettre à jour le temps de la dernière attaque
            lastAttackTime = Time.time;
        }
    }
}
