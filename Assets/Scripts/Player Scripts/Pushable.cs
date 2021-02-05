using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    Rigidbody rb;
    Renderer pushableRenderer;
    [SerializeField] float pushableSize;

    public float PushableSize
    {
        get { return pushableSize; }
    }

    public bool isGrounded;
    float distanceToEdge;
    float edgeBuffer = 0.1f;

    BoxCollider pushableCollider;

    PushObject pushObjectScript;

    void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(IsPushable);

        rb = GetComponent<Rigidbody>();
        pushableRenderer = GetComponent<Renderer>();

        pushableCollider = GetComponent<BoxCollider>();
        distanceToEdge = pushableCollider.bounds.extents.y;

        pushObjectScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PushObject>();
    }

    private void FixedUpdate()
    {
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

        //Debug.DrawRay(transform.position + new Vector3(distanceToEdge, 0, distanceToEdge), Vector3.down * (distanceToEdge + edgeBuffer), Color.red);
    }

    private void Update()
    {
        if(!isGrounded)
        {
            if(pushObjectScript.hasPushable)
               pushObjectScript.EndPush();

            Destroy(GetComponent<FixedJoint>());
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void IsPushable(ChangeColor.PlayerColor currentPlayerColor, ChangeColor.PlayerColor previousPlayerColor)
    {
        if(currentPlayerColor == ChangeColor.PlayerColor.GREEN)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        else if(previousPlayerColor == ChangeColor.PlayerColor.GREEN)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnCollisionStay(Collision collision) //changed from enter to stay
    {
        if(collision.gameObject.CompareTag("Ground") | collision.gameObject.CompareTag("Player")) //not perfect solution but prevents pushables breaking
        {
            FixedJoint fixedJointCheck;

            if(gameObject.TryGetComponent(out fixedJointCheck) == false)
            {
                var fixedJoint = gameObject.AddComponent<FixedJoint>();
                fixedJoint.enableCollision = true;
            }
        }

        if(collision.gameObject.CompareTag("Laser") && !pushObjectScript.hasPushable)
        {
            Destroy(GetComponent<FixedJoint>());
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

   private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            FixedJoint fixedJointCheck;

            if (gameObject.TryGetComponent(out fixedJointCheck) == false)
            {
                var fixedJoint = gameObject.AddComponent<FixedJoint>();
                fixedJoint.enableCollision = true;
            }
        }
    }

}
