using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushedChild : MonoBehaviour
{
    //disolve on laser
    //child dialogue

    [SerializeField] ParticleSystem dust;
    public Animator animator;
    [SerializeField] ChildEventComments childEventComments;

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

    float fallSpeed;
    float fallDelay;

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
            fallSpeed = 5;
            fallDelay = 0;
        }

        //Debug.DrawRay(transform.position + new Vector3(0.95f, 0, -0.95f), pushDirection * collisionDistance, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(-0.95f, 0, -0.95f), pushDirection * collisionDistance, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(0.95f, 0, 0.95f), pushDirection * collisionDistance, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(-0.95f, 0, 0.95f), pushDirection * collisionDistance, Color.red);
    }
    void PushedAway()
    {
        childEventComments.EndComment();
        AudioManager.Instance.PlaySound("PushedAwaySlide");

        if (isGrounded)
        {
            transform.position += pushDirection * 10 * Time.deltaTime;
        }

        else if (!isGrounded)
        {
            fallDelay += Time.deltaTime;

            if(fallDelay < 0.15f)
            {
                transform.position += pushDirection * 10 * Time.deltaTime;
            }

            else if (fallDelay >= 0.15f)
            {
                fallSpeed += 15 * Time.deltaTime;
                transform.position += ((pushDirection * 8) + (new Vector3(0, -1.5f, 0) * fallSpeed)) * Time.deltaTime;
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0.95f, 0, 0.95f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-0.95f, 0, 0.95f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-0.95f, 0, -0.95f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(0.95f, 0, -0.95f), pushDirection, out hit, collisionDistance, collisionMask))
        {
            childPushedAway = false;
            AudioManager.Instance.StopSound("PushedAwaySlide");
            AudioManager.Instance.PlaySound("HardCollision");
            CameraShake.Instance.StartShake(0.2f, 0.2f);
            dust.Stop();
            animator.SetBool("IsPushed", false);
        }
    }

    public void GetDirection()
    {
        pushDirection = new Vector3(transform.position.x - player.position.x, 0, transform.position.z - player.position.z);
        dust.Play();
    }
}
