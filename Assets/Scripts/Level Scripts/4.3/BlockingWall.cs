using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingWall : MonoBehaviour
{
    [SerializeField] LaserTrigger laserTrigger;
    [SerializeField] float raisedPoint;
    [SerializeField] float loweredPoint;
    [SerializeField] float moveSpeed;

    [SerializeField] Transform door;

    enum Wall
    {
        WALL1,
        WALL2
    }

    [SerializeField] Wall wall;

    private void Update()
    {
        if (laserTrigger.Trigger && wall == Wall.WALL1 && door.position.y <= 2.25)
        {
            RaiseWall();
        }
        else if (!laserTrigger.Trigger && wall == Wall.WALL1)
        {
            LowerWall();
        }
        else if(!laserTrigger.Trigger && wall == Wall.WALL2)
        {
            RaiseWall();
        }
        else if (laserTrigger.Trigger && wall == Wall.WALL2 && door.position.y <= 2.25)
        {
            LowerWall();
        }
    }

    void LowerWall()
    {
        if (transform.position.y >= loweredPoint)
        {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

    void RaiseWall()
    {
        if (transform.position.y <= raisedPoint)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

}
