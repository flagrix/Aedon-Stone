using UnityEngine;

public class EnnemyDroit : ennemy {

    public float speed = 10f;

    private Transform target;
    private int waypointIndex = 0;
    void Start()
    {
        target = Waypoints_lane_droite.points[0];
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
        if(waypointIndex >= Waypoints_lane_droite.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints_lane_droite.points[waypointIndex];
    }
}