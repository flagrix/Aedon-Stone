using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FastQwertien : MonoBehaviour
{
    public NavMeshAgent agent2;
    public float stopDistance = 5F; // Distance minimale avant d'arr�ter la poursuite


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

            // Si la distance est sup�rieure � la distance minimale ou que la cible a chang�, recalculer le chemin
            if (distanceToPlayer > stopDistance)
            {
               agent2.SetDestination(targetPos); 
            }
            else
            {
                // Arr�ter le mouvement si l'agent est dans la port�e minimale
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

            // Mettre � jour le temps de la derni�re attaque
            lastAttackTime = Time.time;
        }
    }
}
