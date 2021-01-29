﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Vector3[] movementPoints;
    int movementPointNumber = 0;
    Vector3 currentTarget;

    float tolerance;
    [SerializeField] float speed;
    [SerializeField] float delayTime;
    float delayStart;

    [SerializeField] bool automatic;
    [SerializeField] bool collisionActive = true;

    SphereCollider platformTrigger;
    
    bool trigger;

    public bool Trigger
    {
        get { return trigger; }
        set { trigger = value;  }
    }

    PushObject pushObject;

    private void Start()
    {
        pushObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PushObject>();
        
        PlatformTrigger.OnPlatformTrigger += NextPlatform;
        
        rb = GetComponent<Rigidbody>();

        if (movementPoints.Length > 0)
        {
            currentTarget = movementPoints[0];
        }

        tolerance = speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(transform.position != currentTarget)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    void MovePlatform()
    {
        Vector3 heading = currentTarget - transform.position;
        rb.MovePosition(rb.position + (heading.normalized * speed) * Time.deltaTime);

        if(heading.magnitude < tolerance)
        {
            rb.position = currentTarget;
            delayStart = Time.time;
            trigger = false;
        }
    }

    void UpdateTarget()
    {
        if(automatic)
        {
            if(Time.time - delayStart > delayTime)
            {
                NextPlatform();
            }
        }
    }

    public void NextPlatform()
    {
        trigger = true;
        
        movementPointNumber++;

        if(movementPointNumber >= movementPoints.Length)
        {
            movementPointNumber = 0;
        }

        currentTarget = movementPoints[movementPointNumber];
    }

    private void OnCollisionStay(Collision collision)
    {
        
        
        if(collision.gameObject.CompareTag("Pushable") && collisionActive)
        {
            if(!pushObject.hasPushable)
            {
                collision.gameObject.GetComponent<FixedJoint>().connectedBody = rb;
            }
         }
    }

}

