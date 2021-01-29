using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTriggerDoor : MonoBehaviour
{
    [SerializeField] LaserTrigger laserTrigger;
    [SerializeField] float closedPoint;
    [SerializeField] float openPoint;
    [SerializeField] float moveSpeed;

    private void Update()
    {
        if(laserTrigger.Trigger)
        {
            OpenDoor();
        }
        else if(!laserTrigger.Trigger)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if(transform.position.y >= openPoint)
        {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

    void CloseDoor()
    {
        if(transform.position.y <= closedPoint)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

}
