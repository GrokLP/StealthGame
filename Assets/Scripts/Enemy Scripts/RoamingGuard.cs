using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RoamingGuard : PlayerDetection
{
    private NavMeshAgent roamingGuard;
    [SerializeField] float roamRadius;
    Vector3 destination;
    

    public override void Start()
    {
        base.Start();
        roamingGuard = GetComponent<NavMeshAgent>();

        InvokeRepeating("MoveToDestination", 1, 2);
    }

    public override void Update()
    {
        base.Update();
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void MoveToDestination()
    {
        destination = SetNewDestination();

        roamingGuard.SetDestination(destination);
    }

    Vector3 SetNewDestination()
    {
        var _destination = RandomNavSphere(roamingGuard.transform.position, roamRadius, -1);
        return _destination;      
    }
}
