using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drones : PlayerDetection
{
    [SerializeField] float speed = 5;
    [SerializeField] float waitTime = 0.3f;

    [SerializeField] Transform pathHolder;

    public override void Start()
    {
        base.Start(); 

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); //setting waypoints to gaurd's y so that drone stays at same level in air
        }

        StartCoroutine(GuardPatrol(waypoints));
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition); //completes loop   
    }

    IEnumerator GuardPatrol(Vector3[] waypoints)
    {
        //transform.position = waypoints[0];

        int targetWaypointsIndex = 0;
        Vector3 targetWaypoint = waypoints[targetWaypointsIndex];
        
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointsIndex = (targetWaypointsIndex + 1) % waypoints.Length; //if index reaches array length it's reset to 0 (because remainder is 0)
                targetWaypoint = waypoints[targetWaypointsIndex]; //set next target
                yield return new WaitForSeconds(waitTime);
            }
            yield return null; //ensures it only travels max distance per frame
        }
    }


}
