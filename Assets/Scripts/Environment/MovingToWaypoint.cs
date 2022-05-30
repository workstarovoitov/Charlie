using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingToWaypoint : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private MotionType type;

    private float waypointReachTime = 0f;
    private void FixedUpdate()
    {
 
        if (Vector3.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            waypointReachTime = Time.time;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
            
        }
        if (Time.time < waypointReachTime + waitTime) return;
        switch (type)
        {
            case MotionType.T1:
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
                break;
            case MotionType.T2:
                transform.position = Vector3.Lerp(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
                break;
        }
    }


    enum MotionType
    {
        T1,
        T2,
    }
}
