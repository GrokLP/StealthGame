using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformsFiveOne : MonoBehaviour
{
    [SerializeField] LaserTrigger trigger;
    [SerializeField] float positionOne;
    [SerializeField] float positionTwo;
    [SerializeField] float moveSpeed;

    bool movePlatform;

    private void Update()
    {
        if (trigger.Trigger && movePlatform)
        {
            if (transform.position.x == positionOne)
            {
                movePlatform = false;
            }

            MoveRight();
        }

        else if (trigger.Trigger && !movePlatform)
        {
            if (transform.position.x == positionTwo)
            {
                movePlatform = true;
            }

            MoveLeft();
        }
    }

    void MoveRight()
    {
        if(transform.position.x <= positionOne)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(positionOne, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }

    void MoveLeft()
    {
        if (transform.position.x >= positionTwo)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(positionTwo, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }
}
