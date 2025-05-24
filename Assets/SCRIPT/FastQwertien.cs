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

    private int Health = 80;
    private void Awake()
    {
       
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
