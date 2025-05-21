using UnityEngine;

public class ennemy : MonoBehaviour
{
    public int worth = 50;
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
        Inventory_global inv = FindObjectOfType<Inventory_global>();
        if (inv != null)
        {
            inv.addRune(worth);
        }
        else
        {
            Debug.LogWarning("Inventory_global non trouvé !");
        }
        isDead = true;
        Destroy(gameObject);
    }
    

      public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
    }




}