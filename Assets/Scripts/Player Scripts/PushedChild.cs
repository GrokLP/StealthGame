using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushedChild : MonoBehaviour
{
    //animations and particle effects
    //screen shake on collision
    //child dialogue

    public Transform forward;
    public Transform back;
    public Transform right;
    public Transform left;


    float collisionDistance = 0.15f;
    [SerializeField] LayerMask collisionMask;

    Vector3 pushDirection;

    float distanceToEdge;
    BoxCollider pushableCollider;
    float edgeBuffer = 0.1f;
    bool isGrounded;

    bool childPushedAway;
    public bool ChildPushedAway
    {
        get { return childPushedAway; }
        set { childPushedAway = value; }
    }

    Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        pushableCollider = GetComponent<BoxCollider>();
        distanceToEdge = pushableCollider.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        if (childPushedAway)
        {
            PushedAway();
        }

        if (!Physics.Raycast(transform.position + new Vector3(distanceToEdge, 0, distanceToEdge), Vector3.down, distanceToEdge + edgeBuffer)
            && !Physics.Raycast(transform.position + new Vector3(distanceToEdge, 0, -distanceToEdge), Vector3.down, distanceToEdge + edgeBuffer)
            && !Physics.Raycast(transform.position + new Vector3(-distanceToEdge, 0, distanceToEdge), Vector3.down, distanceToEdge + edgeBuffer)
            && !Physics.Raycast(transform.position + new Vector3(-distanceToEdge, 0, -distanceToEdge), Vector3.down, distanceToEdge + edgeBuffer))
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }

        //Debug.DrawRay(transform.position + new Vector3(1f, 0, -1f), pushDirection * collisionDistance, Color.red);
        // Debug.DrawRay(transform.position + new Vector3(-1f, 0, -1f), pushDirection * collisionDistance, Color.red);
        // Debug.DrawRay(transform.position + new Vector3(1f, 0, 1f), pushDirection * collisionDistance, Color.red);
        // Debug.DrawRay(transform.position + new Vector3(-1f, 0, 1f), pushDirection * collisionDistance, Color.red);
    }
    void PushedAway()
    {
        if (isGrounded)
            transform.position += pushDirection * 10 * Time.deltaTime;
        else if (!isGrounded)
            transform.position += ((pushDirection * 7 ) + (new Vector3(0, -1.5f, 0) * 10)) * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(1f, 0, 1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-1f, 0, 1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-1f, 0, -1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(1f, 0, -1f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(1f, 0, 1f), -pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-1f, 0, 1f), -pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-1f, 0, -1f), -pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(1f, 0, -1f), -pushDirection, out hit, collisionDistance, collisionMask))
        {
            childPushedAway = false;
        }
    }

    public void GetDirection()
    {
        pushDirection = new Vector3(transform.position.x - player.position.x, 0, transform.position.z - player.position.z);
    }
}
