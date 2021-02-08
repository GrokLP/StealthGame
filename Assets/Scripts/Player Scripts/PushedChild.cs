using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushedChild : MonoBehaviour
{
    //freeze rotation and have player snap to position on grab?
    //animations and particle effects
    //screen shake on collision
    //child dialogue

    float collisionDistance = 0.1f;
    [SerializeField] LayerMask collisionMask;

    Vector3 pushDirection;
    public Vector3 PushDirection
    {
        set { pushDirection = value; }
    }

    bool childPushedAway;
    public bool ChildPushedAway
    {
        get { return childPushedAway; }
        set { childPushedAway = value; }
    }

    private void FixedUpdate()
    {
        if (childPushedAway)
        {
            PushedAway();
        }

        //Debug.DrawRay(transform.position + new Vector3(1f, 0, -1f), pushDirection * collisionDistance, Color.red);
       // Debug.DrawRay(transform.position + new Vector3(-1f, 0, -1f), pushDirection * collisionDistance, Color.red);
       // Debug.DrawRay(transform.position + new Vector3(1f, 0, 1f), pushDirection * collisionDistance, Color.red);
       // Debug.DrawRay(transform.position + new Vector3(-1f, 0, 1f), pushDirection * collisionDistance, Color.red);
    }
    void PushedAway()
    {
        transform.position += pushDirection * 10 * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(1f, 0, 1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-1f, 0, 1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-1f, 0, -1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(1f, 0, -1f), pushDirection, out hit, collisionDistance, collisionMask))
        {
            childPushedAway = false;
        }
    }
}
