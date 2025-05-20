using UnityEngine;

public class ennemy : MonoBehaviour
{
     public int health = 100;
    public void TakeDammage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }
    
     private void Die()
    {
        Destroy(gameObject);
    }



}