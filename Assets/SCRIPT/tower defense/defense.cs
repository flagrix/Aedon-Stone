﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using TMPro;

public class defense : MonoBehaviourPunCallbacks
{

    private Transform target;
    private ennemy targetEnemy;
    public float range = 15f;

    public string enemyTag = "Enemy";

    public Transform partToRotate;

    private float turnSpeed = 6.5f;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public float Damage = 50f;

    public LineRenderer lineRenderer;
    public bool useLaser;

    public int damageOverTime = 20;

    public int damageOverTime_tde = 27;
    public float slowAmount = 0.5f;

    public AudioSource WazeAudioSource;
    public AudioClip TDE_sound;
    public AudioClip canon_sound;
    public AudioClip balise_sound;

    public bool isCanon = false;
    public bool isBalise = true;

    public int level = 0;

    public float damageUpgradeAmount = 15f;
    public float damageUpgradeAmount_tde = 7f;
    public float rangeUpgradeAmount = 3f;

    public int upgradeCost = 200;

    public int upgradeCost_tde = 300;
    public int upgradeCostIncrement = 100;

    public float baseDamage = 50f;
    public float baseRange = 15f;
    public float Damagenext = 90f;
    public float Rangenext = 20f;

    public Inventory_global inv;

    public PhotonView PV;

    public bool isupgratable;


    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        inv = FindObjectOfType<Inventory_global>();
        PV = GetComponent<PhotonView>();
    }


    public void Initialize()
    {

    }


    void UpdateTarget()
    {
        GameObject[] ennemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in ennemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = target.GetComponent<ennemy>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }

            if (WazeAudioSource.isPlaying && WazeAudioSource.clip == TDE_sound)
            {
                Debug.Log("Stop laser sound (plus de cible)");
                WazeAudioSource.Stop();
            }
            return;
        }

        LockOnTarget();
        if (useLaser)
        {
            Laser();
            if (WazeAudioSource.clip != TDE_sound)
            {
                WazeAudioSource.clip = TDE_sound;
            }

            if (!WazeAudioSource.isPlaying)
            {
                WazeAudioSource.Play();
            }
        }
        else
        {
            if (WazeAudioSource.isPlaying && WazeAudioSource.clip != canon_sound && WazeAudioSource.clip != balise_sound)
            {
                Debug.Log("arrete son tde");
                WazeAudioSource.Stop();
            }
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1 / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Shoot()
    {
        if (isBalise)
        {
            if (WazeAudioSource.clip != balise_sound)
            {
                WazeAudioSource.clip = balise_sound;
            }

            if (!WazeAudioSource.isPlaying)
            {
                WazeAudioSource.Play();
            }
            GameObject bulletGO = (GameObject)PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "fleche"), firePoint.position, firePoint.rotation, 0);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.damage = Damage;
                bullet.Seek(target);
            }
        }
        else if (isCanon)
        {
            if (WazeAudioSource.clip != canon_sound)
            {
                WazeAudioSource.clip = canon_sound;
            }

            if (!WazeAudioSource.isPlaying)
            {
                WazeAudioSource.Play();
            }
            GameObject bulletGO = (GameObject)PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "boulet_de_canon"), firePoint.position, firePoint.rotation, 0);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.damage = Damage;
                bullet.Seek(target);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    [PunRPC]
    public void RPC_Upgrade()
    {
        if (isCanon || isBalise)
        {

            if (Inventory_global.runes >= upgradeCost)
            {
                inv.addRune(-upgradeCost_tde);
                level++;

                Damage = Damage + (damageUpgradeAmount * (level));
                range = range + (rangeUpgradeAmount * (level));
                Damagenext = Damage + (damageUpgradeAmount * (level + 1));
                Rangenext = range + (damageUpgradeAmount * (level + 1));
                upgradeCost += upgradeCostIncrement; ;
                isupgratable = true;
            }
        }
            else if (useLaser)
            {
                if (Inventory_global.runes >= upgradeCost_tde)
                {
                    inv.addRune(-upgradeCost_tde);
                    level++;

                    damageOverTime = (int)(damageOverTime + (damageUpgradeAmount_tde * (level)));
                    range = range + (rangeUpgradeAmount * (level));
                    damageOverTime_tde = (int)(damageOverTime + (damageUpgradeAmount_tde * (level + 1)));
                    Rangenext = range + (damageUpgradeAmount * (level + 1));
                    upgradeCost_tde += upgradeCostIncrement;
                }
            }

    }

    public void Upgrade()
    {
        PV.RPC("RPC_Upgrade", RpcTarget.All);
    }
    
     private void OnMouseDown()
    {
        foreach (var inventory in FindObjectsOfType<Inventory>())
        {
            if (inventory.photonView.IsMine)
            {
                inventory.ShowUpgradePanel(this);
                break;
            }
        }

    }
    
}
