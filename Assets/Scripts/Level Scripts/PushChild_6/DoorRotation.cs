using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{

    [SerializeField] DoorRotationSwitch rotationSwitch;
    [SerializeField] float openPoint;
    [SerializeField] float closedPoint;
    [SerializeField] float rotationSpeed;

    void Update()
    {
        if(rotationSwitch.OpenDoor)
        {
            OpenDoor();
        }
        else if(!rotationSwitch.OpenDoor)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, openPoint)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, openPoint, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
        }
    }

    void CloseDoor()
    {
        if(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, closedPoint)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, closedPoint, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
        }

    }
}
