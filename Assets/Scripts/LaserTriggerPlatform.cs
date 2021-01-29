using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTriggerPlatform : MonoBehaviour
{
    public static event System.Action<bool> OnNextPlatform;
    
    Rigidbody rb;

    [SerializeField] Vector3[] movementPoints;
    int movementPointNumber = 0;
    Vector3 currentTarget;

    float tolerance;
    [SerializeField] float speed;
    [SerializeField] int endpoint;

    PushObject pushObject;

    [SerializeField] LaserTrigger laserTrigger;

    private void Start()
    {
        pushObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PushObject>();

        rb = GetComponent<Rigidbody>();

        if (movementPoints.Length > 0)
        {
            currentTarget = movementPoints[0];
        }

        tolerance = speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (transform.position != currentTarget)
        {
            MovePlatform();
        }
        else if (!laserTrigger.Trigger && movementPointNumber == 0)
        {
            return;
        }
        else if (laserTrigger.Trigger && movementPointNumber == endpoint)
        {
            return;
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

        if (heading.magnitude < tolerance)
        {
            rb.position = currentTarget;
        }
    }

    void UpdateTarget()
    {
        if (laserTrigger.Trigger && movementPointNumber != endpoint)
        {
            NextPlatform();
        }
        else if (!laserTrigger.Trigger)
        {
            PreviousPlatform();
        }
           
    }

    public void NextPlatform()
    {
        if(OnNextPlatform != null)
            OnNextPlatform(true);
        
        movementPointNumber++;

        if (movementPointNumber >= movementPoints.Length)
        {
            movementPointNumber = 0;
        }

        currentTarget = movementPoints[movementPointNumber];
    }

    public void PreviousPlatform()
    {
        if (OnNextPlatform != null)
            OnNextPlatform(false);
        
        movementPointNumber--;

        if (movementPointNumber <= 0)
        {
            movementPointNumber = 0;
        }

        currentTarget = movementPoints[movementPointNumber];
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pushable"))
        {
            if (!pushObject.hasPushable)
            {
                collision.gameObject.GetComponent<FixedJoint>().connectedBody = rb;
            }
        }
    }
}
