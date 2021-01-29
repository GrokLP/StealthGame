using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformsFourFour : MonoBehaviour
{
    [SerializeField] LaserTrigger trigger;
    [SerializeField] float highPoint;
    [SerializeField] float lowPoint;
    [SerializeField] float moveSpeed;

    bool raisePlatform;

    private void Update()
    {
        if(trigger.Trigger && raisePlatform)
        {
            if (transform.position.y == highPoint)
            {
                raisePlatform = false;
            }

            RaisePlatform();
        }

        else if(trigger.Trigger && !raisePlatform)
        {
            if (transform.position.y == lowPoint)
            {
                raisePlatform = true;
            }

            LowerPlatform();
        }
    }

    void LowerPlatform()
    {
        if(transform.position.y >= lowPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, lowPoint, transform.position.z), moveSpeed * Time.deltaTime);
        } 
    }

    void RaisePlatform()
    {
        if(transform.position.y <= highPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, highPoint, transform.position.z), moveSpeed * Time.deltaTime);

        }
    }


}
