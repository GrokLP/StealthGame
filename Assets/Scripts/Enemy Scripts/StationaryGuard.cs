﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StationaryGuard : MonoBehaviour
{
    [SerializeField] Animator enemyDissolve;
    
    [SerializeField] float hearingRadius;
    [SerializeField] float childHearingRadius;
    [SerializeField] float patrolResetLimit;
    [SerializeField] NavMeshAgent stationaryGuard;

    [SerializeField] float turnSpeed;

    [SerializeField] GameObject exclamationMark;
    [SerializeField] GameObject questionMark;
    [SerializeField] GameObject smiley;

    bool alreadyMoving;

    private Vector3 target;
    public Vector3 Target
    {
        get { return target; }
        set
        {
            target = value;
           
            if(Vector3.Distance(transform.position, target) <= hearingRadius && !alreadyMoving)
            {
                StopCoroutine("MoveTowards");
                StartCoroutine("MoveTowards", target);
            }
        }
    }

    private Vector3 childTarget;
    public Vector3 ChildTarget
    {
        get { return target; }
        set
        {
            childTarget = value;

            if (Vector3.Distance(transform.position, childTarget) <= childHearingRadius && !alreadyMoving)
            {
                StopCoroutine("MoveTowards");
                StartCoroutine("MoveTowards", childTarget);
            }
        }
    }


    IEnumerator GuardRotation(Vector3 lookTarget) 
     {
          Vector3 dirToLookTarget = (lookTarget - transform.position).normalized; //find direction to look in by subtracting guards current position from target position
         float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

         while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) // if rotation isn't finshed, keep rotating (setting to 0 can cause issues because sometimes it's not exact)
         {
             float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime); //rotate towards target angle
             transform.eulerAngles = Vector3.up * angle; //rotate around y axis
             yield return null; //ensures it only travels max distance per frame
         }
        
     }

    IEnumerator ReturnToStartPos(Vector3 _startingPos, Quaternion _startingRot)
    {
        questionMark.SetActive(false);
        AudioManager.Instance.PlaySound("GuardSatisfied");

        yield return new WaitForSeconds(0.5f);

        smiley.SetActive(true);

        stationaryGuard.stoppingDistance = 0.5f;

        stationaryGuard.SetDestination(_startingPos);

        while (Vector3.Distance(stationaryGuard.transform.position, _startingPos) >= stationaryGuard.stoppingDistance)
            yield return null;

        yield return new WaitForSeconds(1);

        transform.rotation = _startingRot;
        alreadyMoving = false;
        smiley.SetActive(false);
    }

    IEnumerator MoveTowards(Vector3 target) 
    {
        alreadyMoving = true;
        float timer = 0;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;
        float randomLookX = Random.Range(-50, 50);
        float randomLookZ = Random.Range(-50, 50);

        yield return new WaitForSeconds(1f);//delay for thorwn object animation to play

        exclamationMark.SetActive(true);
        AudioManager.Instance.PlaySound("GuardDistracted");

        yield return new WaitForSeconds(1f);

        exclamationMark.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        questionMark.SetActive(true);

        stationaryGuard.stoppingDistance = 1f; //**Can maybe remove this now? Was having issues with stopping distance but it appears to be resolved

        stationaryGuard.SetDestination(target); // + new Vector3(0, 0.5f, 0)

        while (Vector3.Distance(stationaryGuard.transform.position, target) >= stationaryGuard.stoppingDistance)
        {
            yield return null;
            timer += Time.deltaTime;
            
            if(timer > patrolResetLimit)
            {
                StartCoroutine(ReturnToStartPos(startingPos, startingRot));
                yield break;
            }
        }
 
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(GuardRotation(transform.position + new Vector3(randomLookX, 0, randomLookZ))); 
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(GuardRotation(transform.position + new Vector3(-randomLookX, 0, -randomLookZ)));
        yield return new WaitForSeconds(1f);

        StartCoroutine(ReturnToStartPos(startingPos, startingRot));
    }

    public void StopCoroutines()
    {
        StopAllCoroutines();
        stationaryGuard.isStopped = true;
    }

    private void OnDestroy()
    {
        var throwObjectScript = GameObject.FindObjectOfType<ThrowObject>();
        if(throwObjectScript != null)
            throwObjectScript.RemoveGuardOnDestroy(this);
    }
}
