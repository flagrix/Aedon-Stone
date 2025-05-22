using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FatQwertien : MonoBehaviour
{
    public NavMeshAgent agent2;
    public float stopDistance = 8F; // Distance minimale avant d'arrêter la poursuite

    private Vector3 lastTargetPosition;
    public Slider healthBar;
    public int attackDamage = 15;
    public float attackCooldown = 5f;
    private float lastAttackTime;
    public static FatQwertien instance;

    private int Health = 200;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

  /*  void Update()
    {
        float interetjoueur = -Vector3.Distance(agent2.transform.position, PlayerMovement.instance.GetPlayerPosition());// - HealthBar.instance.actual_health;
        float interettour = -Vector3.Distance(agent2.transform.position, new Vector3(414.6f, 2f, 626.3f)) -  TowerHealth.instance.health;
        if (PlayerMovement.instance != null)
        {
            Vector3 targetPos = new Vector3(0f,0f,0f);
            if (interetjoueur > interettour)
            {
                targetPos = PlayerMovement.instance.GetPlayerPosition();
            }
            else
            {
                targetPos = new Vector3(414.6f, 2f, 626.3f);
            }

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

                agent2.ResetPath();
                //if (!GameOver.instance.isGameOver)
                    if (interetjoueur > interettour)
                        AttackPlayer();
                    else AttackTower();
            }
        }
    }*/

    public void SetHealth(int i)
    {
        Debug.Log(Health);
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

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            /**if (HealthBar.instance != null)
            {
                HealthBar.instance.SetActualHealth(-attackDamage);
                Debug.Log("Qwertien attaque le joueur !");
            }**/

            // Mettre à jour le temps de la dernière attaque
            lastAttackTime = Time.time;
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
