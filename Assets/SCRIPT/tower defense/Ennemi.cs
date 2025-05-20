using UnityEngine;

public class ennemy : MonoBehaviour
{
    public float startSpeed = 10;
    public float speed;
    public float startHealth = 100;
    public float health;
    private bool isDead = false;

    public void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }
    public void TakeDammage(float amount)
    {
        health -= amount;

        if(health <= 0 || isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
    

      public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
    }




}