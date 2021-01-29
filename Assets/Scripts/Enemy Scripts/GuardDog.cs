using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardDog : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;

    private NavMeshAgent roamingGuardDog;

    [SerializeField] float roamRadius;
    [SerializeField] float sniffTime;
    [SerializeField] float chaseSpeed;
    [SerializeField] float chaseAcceleration;
    [SerializeField] float roamSpeed;
    [SerializeField] float roamAcceleration;
    [SerializeField] float playerEscapeDistance;
    [SerializeField] float tooCloseDistance;

    [SerializeField] Animator guardDogSniff;

    Vector3 roamingDestination;
    Transform playerPosition;
    bool playerDetected;
    bool isSniffing;

    void Start()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;

        roamingGuardDog = GetComponent<NavMeshAgent>();

        StartCoroutine(Roaming());
    }

    IEnumerator Roaming()
    {
        while(true)
        {
            roamingGuardDog.isStopped = false;
            roamingGuardDog.acceleration = roamAcceleration;
            roamingGuardDog.speed = roamSpeed;

            while (!playerDetected)
            {
                isSniffing = false;
                guardDogSniff.SetBool("IsSniffing", false);
                MoveToDestination();
                
                while (Vector3.Distance(roamingGuardDog.transform.position, roamingDestination) >= roamingGuardDog.stoppingDistance)
                {
                    yield return null;
                }
                    
                //dog is sniffing

                isSniffing = true;
                guardDogSniff.SetBool("IsSniffing", true);
                yield return new WaitForSeconds(sniffTime);
            }

            //Bark and Chase -- could still add animation and/or sound

            while (playerDetected && Vector3.Distance(roamingGuardDog.transform.position, playerPosition.position) <= playerEscapeDistance)
            {
                isSniffing = false;
                guardDogSniff.SetBool("IsSniffing", false);
                ChasePlayer();

                if (Vector3.Distance(transform.position, playerPosition.position) < tooCloseDistance)
                    if (OnGameLose != null)
                    {
                        OnGameLose("GuardDog");
                    }

                yield return null;
            }
            //Player Escaped -- maybe have some kind of sound when escaped?

            roamingGuardDog.isStopped = true;
            playerDetected = false;
            
            yield return null;
        }
    }

    void MoveToDestination()
    {
        roamingDestination = SetNewDestination();

        roamingGuardDog.SetDestination(roamingDestination);
    }

    Vector3 SetNewDestination()
    {
        var _destination = RandomNavSphere(roamingGuardDog.transform.position, roamRadius, -1);
        return _destination;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && isSniffing)
        {
            playerDetected = true;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    void ChasePlayer()
    {
        roamingGuardDog.acceleration = chaseAcceleration;
        roamingGuardDog.speed = chaseSpeed;
        roamingGuardDog.SetDestination(playerPosition.position);
    }

}

