using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public static event System.Action OnPushStart;
    public static event System.Action OnPushEnd;

    Pushable pushable;
    GameObject pushableObject;
    Rigidbody rb;
    Pushable pushableScript;

    [SerializeField] float distance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float baseMoveSpeed;
    float pushableMoveSpeed;

    [SerializeField] Material pushed;
    [SerializeField] Material notPushed;

    Vector3 forward, right;

    bool materialChanged;
    public bool hasPushable;

    Renderer rend;

    bool gameEnd;

    bool isActive;

    [SerializeField] BoxCollider boxCollider;
    Vector3 normalBoxSize;
    Vector3 smallBoxSize;
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(HandleColorChange);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChange);

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        normalBoxSize = boxCollider.size;
        smallBoxSize = boxCollider.size * 0.85f; //reduce hitbox while pushing so that small corners hanging out arent punished
    }


    void Update()
    {
        if (Input.GetButton("Push") && !gameEnd && isActive)
        {
            StartPush();
            if (hasPushable)
                Movement();
        }

        else if (Input.GetButtonUp("Push"))
        {
            EndPush();
        }
    }

    void StartPush()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layerMask))
        {
            boxCollider.size = smallBoxSize;
            
            pushableScript = hit.collider.gameObject.GetComponent<Pushable>();

            if (hit.collider != null && hit.collider.CompareTag("Pushable") && pushableScript.isGrounded)
            {
                if (OnPushStart != null)
                    OnPushStart();

                pushableObject = hit.collider.gameObject;

                if (!materialChanged)
                {
                    rend = pushableObject.GetComponent<Renderer>();
                    rend.material = pushed;
                    materialChanged = true;
                }

                pushableObject.GetComponent<FixedJoint>().connectedBody = rb;

                rb.constraints = RigidbodyConstraints.FreezeRotation;

                var pushableSize = hit.collider.GetComponent<Pushable>().PushableSize;

                if (!hasPushable)
                {
                    pushableMoveSpeed = (baseMoveSpeed / pushableSize) + 0.5f;
                    hasPushable = true;
                }
            }
        }
    }

    public void EndPush()
    {
        if(boxCollider != null)
            boxCollider.size = normalBoxSize;
        
        if (OnPushEnd != null)
            OnPushEnd();

        if (materialChanged)
        {
            rend.material = notPushed;
            materialChanged = false;
        }

        if (hasPushable)
            pushableObject.GetComponent<FixedJoint>().connectedBody = null;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        hasPushable = false;
    }

    void Movement()
    {
        Vector3 rightMovement = right * pushableMoveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = forward * pushableMoveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        transform.position += (rightMovement + forwardMovement);
    }

    void HandleColorChange(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        if (previousColor == ChangeColor.PlayerColor.GREEN)
            EndPush();
    }

    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (currentState == GameManager.GameState.GAMEWIN)
            gameEnd = true;
        else if (currentState == GameManager.GameState.GAMELOSE)
            gameEnd = true;
    }
}