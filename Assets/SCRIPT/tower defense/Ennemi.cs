using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ennemy : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] public PhotonView PV;
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
        if (PV == null)
            PV = GetComponentInParent<PhotonView>();
        if (PV == null)
            PV = GetComponentInChildren<PhotonView>();
         object[] instantiationData = photonView.InstantiationData;
        if (instantiationData != null && instantiationData.Length > 0)
        {
            int wave = (int)instantiationData[0];
            float multiplier = 1f + (wave * 0.2f); // 20% de PV en plus par vague
            startHealth *= multiplier;
            health = startHealth;
        }
        PV.RPC("SetHealth", RpcTarget.All);
    }

    public void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        speed = startSpeed;
        health = startHealth;
    }
    public void TakeDamage(float amount)
    {
        if (PV != null)
            PV.RPC("RPC_TakeDamage", RpcTarget.All, amount);

    }

    public void FinalTakeDamage(float amount)
    {
        health -= amount;
        PV.RPC("SetHealth", RpcTarget.All);
        if (health <= 0 || isDead)
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
        if (photonView.IsMine)
        {
            Debug.Log(gameObject.name + " is dead");
            if (gameObject.GetComponent<Ouvrier>() == null)
                PhotonNetwork.Destroy(gameObject);
        }
    }


    public void Slow(float amount)
    {
        speed = startSpeed * (1f - amount);
    }
    
      




}