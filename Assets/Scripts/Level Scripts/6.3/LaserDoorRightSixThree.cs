using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoorRightSixThree : MonoBehaviour
{
    [SerializeField] LaserTrigger laserTrigger;
    [SerializeField] float closedPoint;
    [SerializeField] float openPoint;
    [SerializeField] float moveSpeed;

    private void Update()
    {
        if (laserTrigger.Trigger)
        {
            CloseDoor();
        }
        else if (!laserTrigger.Trigger)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        if (transform.position.z >= openPoint)
        {
            transform.position -= Vector3.forward * moveSpeed * Time.deltaTime;
        }
    }

    void CloseDoor()
    {
        if (transform.position.z <= closedPoint)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
    }

}
