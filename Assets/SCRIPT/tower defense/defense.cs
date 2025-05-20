using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class defense : MonoBehaviour {

    private Transform target;
    public float range = 15f;

    public string enemyTag = "Enemy";

    public Transform partToRotate;

    private float turnSpeed = 6.5f;

    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    
    public float Damage = 50f;

// Use this for initialization
    void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
}

    void UpdateTarget()
    {
        GameObject[] ennemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in ennemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

// Update is called once per frame
void Update () {
if(target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1 / fireRate;
        }

        fireCountdown -= Time.deltaTime;
}

    void Shoot()
    {
        GameObject bulletGO = (GameObject)PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "boulet_de_canon"), firePoint.position,firePoint.rotation, 0);;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.damage = Damage;
            bullet.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
