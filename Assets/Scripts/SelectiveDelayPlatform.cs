using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectiveDelayPlatform : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Vector3[] movementPoints;
    int movementPointNumber = 0;
    Vector3 currentTarget;

    float tolerance;
    [SerializeField] float speed;
    [SerializeField] float delayTime;
    float delayStart;

    [SerializeField] bool selectiveDelay;
    [SerializeField] int[] delayPoints;

    SphereCollider platformTrigger;

    public bool trigger; //make property

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
        if (transform.position != currentTarget)
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

        if (heading.magnitude < tolerance)
        {
            rb.position = currentTarget;
            delayStart = Time.time;
        }
    }

    void UpdateTarget()
    {
        if (selectiveDelay)
        {
            if (delayPoints[0] == movementPointNumber && Time.time - delayStart > delayTime) //starting point
            {
                NextPlatform();
            }
            else if ((delayPoints[1]) == movementPointNumber && Time.time - delayStart > delayTime) //second delay point
            {
                NextPlatform();
            }
            else if (delayPoints[0] != movementPointNumber && delayPoints[1] != movementPointNumber)
            {
                NextPlatform();
            }
        }
    }

    public void NextPlatform()
    {
        movementPointNumber++;

        if (movementPointNumber >= movementPoints.Length)
        {
            movementPointNumber = 0;
        }

        currentTarget = movementPoints[movementPointNumber];
    }

    public void PreviousPlatform()
    {
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
