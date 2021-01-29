using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGuard : PlayerDetection
{
    [SerializeField] Animator enemyDissolve;
    
    //guard movement parameters
    [SerializeField] float speed = 5;
    [SerializeField] float waitTime = 0.3f;
    [SerializeField] float turnSpeed = 90;

    [SerializeField] Transform pathHolder;

    //[SerializeField] Animator waypointGuardAnimator;

    public override void Start()
    {
        base.Start(); //before, the new start function i had replaced the base start function -- making it virtual allows me to override with additional code while keeping base!

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); //setting waypoints to gaurd's y so that gaurd isnt sunk into floor
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
        transform.position = waypoints[0]; //start at first waypoint

        int targetWaypointsIndex = 0;
        Vector3 targetWaypoint = waypoints[targetWaypointsIndex];
        transform.LookAt(targetWaypoint);

        //waypointGuardAnimator.SetTrigger("Patrolling");

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointsIndex = (targetWaypointsIndex + 1) % waypoints.Length; //if index reaches array length it's reset to 0 (because remainder is 0)
                targetWaypoint = waypoints[targetWaypointsIndex]; //set next target
                yield return new WaitForSeconds(waitTime);

                yield return StartCoroutine(GuardRotation(targetWaypoint)); //yeild return ensures the rotation finishes before it continues

            }
            yield return null; //ensures it only travels max distance per frame
        }
    }

    IEnumerator GuardRotation(Vector3 lookTarget) //pass in target co-ordinates when called
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized; //find direction to look in by subtracting guards current position from target position
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg; //calculate angle to rotate using Atan2 function (subtract from 90 because unity faces foward along z access by default -- could also just flip x and z)
        
        //yield return new WaitForSeconds(0.25f);

        //waypointGuardAnimator.SetTrigger("StartRotation");

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) // if rotation isn't finshed, keep rotating (setting to 0 can cause issues because sometimes it's not exact)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime); //rotate towards target angle
            transform.eulerAngles = Vector3.up * angle; //rotate around y axis
            yield return null; //ensures it only travels max distance per frame
        }

        //waypointGuardAnimator.SetTrigger("FinishRotation");
    }
}
