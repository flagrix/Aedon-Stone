using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ennemy : MonoBehaviourPunCallbacks, IDamageable
{
    PhotonView PV;
    public int worth = 50;
    public float startSpeed = 10;
    public float speed;
    public float startHealth = 100;
    public float health;
    public int attackDamage = 10;
    public float attackCooldown = 4f; 
    private bool isDead = false;
    public Slider healthBar;

    public void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }
    public void TakeDamage(float amount)
    {
        if (PV != null)
            PV.RPC("RPC_TakeDamage", RpcTarget.All, amount);
            PV.RPC("SetHealth", RpcTarget.All, health);
    }

    public void FinalTakeDamage(float amount)
    {
        if (!PV.IsMine)return;
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
      
    void RPC_TakeDamage(float amount)
    {
        
    }




}