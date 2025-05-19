using UnityEngine;

public class Waypoints_lane_mid : MonoBehaviour {

    public static Transform[] points_mid;

    void Awake () {
        points_mid = new Transform[transform.childCount];
        for (int i = 0; i < points_mid.Length; i++)
        {
            points_mid[i] = transform.GetChild(i);
        }
    }
}
