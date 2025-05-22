using Photon.Pun;
using UnityEngine;

public class EnnemyGauche : ennemy {
    
    private Transform target;
    private int waypointIndex = 0;

    void Start()
    {
        target = Waypoints_lane_gauche.points[0];
    }
    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.3f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints_lane_gauche.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints_lane_gauche.points[waypointIndex];
    }
    [PunRPC]
    public void RPC_TakeDamage(float dmg)
    {
        base.FinalTakeDamage(dmg);
    }
    
    [PunRPC]
    public void SetHealth()
    {
        if (healthBar != null)
            healthBar.value = health;

        if (health <= 0)
        {
            if (photonView.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }
 }
