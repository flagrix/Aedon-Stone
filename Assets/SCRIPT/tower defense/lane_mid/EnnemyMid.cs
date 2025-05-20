using UnityEngine;

public class EnnemyMid : ennemy {

    public float speed = 10f;

    private Transform target;
    private int waypointIndex = 0;


    void Start()
    {
        target = Waypoints_lane_mid.points_mid[0];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.3f)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints_lane_mid.points_mid.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints_lane_mid.points_mid[waypointIndex];
    }
}
