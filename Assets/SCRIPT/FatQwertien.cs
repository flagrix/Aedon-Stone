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
