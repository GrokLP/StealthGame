using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class BetterJump : MonoBehaviour
{
    [SerializeField] ParticleSystem jumpDust;
    [SerializeField] ParticleSystem landDust;
    [SerializeField] Animator playerAnimator;

    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] float jumpVelocity;

    [SerializeField] float groundedSkin = 0.05f;
    [SerializeField] LayerMask mask;
    [SerializeField] Rectangle fakeShadow;

    [SerializeField] float coyoteTime = 0.1f;
    float coyoteCounter;

    //[SerializeField] float jumpBufferLength;
    //float jumpBufferCount;

    bool jumpRequest;
    bool isGrounded;
    bool hasJumped;

    float startJump;
    float endJump;
    float jumpTime;

    Vector3 playerSize;
    Vector3 boxSize;
    
    Rigidbody rb;

    bool isActive;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        playerSize = GetComponent<BoxCollider>().size;
        boxSize = new Vector3(playerSize.x, groundedSkin, playerSize.z);
    }

    void Update()
    {
        Vector3 boxCenter = (Vector3)transform.position + Vector3.down * (playerSize.y + boxSize.y) * 0.5f;
        isGrounded = Physics.CheckBox(boxCenter, boxSize, Quaternion.identity, mask);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        /*if(Input.GetButtonDown("Jump") && isActive)
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }*/


        if (Input.GetButtonDown("Jump") && isActive && coyoteCounter > 0f)
        {
            jumpRequest = true;
            startJump = Time.time;
            //jumpBufferCount = 0;
            coyoteCounter = 0;
            JumpDust();
            playerAnimator.SetTrigger("Jump");
            AudioManager.Instance.PlaySound("PlayerJump");
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f) && GameManager.Instance.CurrentGameState != GameManager.GameState.GAMELOSE && fakeShadow != null)
        {
            fakeShadow.transform.position = hit.point + new Vector3(0, 0.01f, 0);
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            rb.AddForce (Vector3.up * jumpVelocity, ForceMode.Impulse);
            jumpRequest = false;
            isGrounded = false;
            hasJumped = true;
        }

        if (rb.velocity.y < 0 && coyoteCounter <= 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
           rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void JumpDust()
    {
        jumpDust.Play();
    }

    void LandDust()
    {
        landDust.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var radial = landDust.velocityOverLifetime;
        
        if (collision.gameObject.CompareTag("Ground") && isGrounded && hasJumped)
        {
            endJump = Time.time;
            jumpTime = endJump - startJump;
            radial.radial = jumpTime * 10;
            LandDust();
            hasJumped = false;
            AudioManager.Instance.PlaySound("PlayerJumpLanding");

        }

    }
}

/* ALTERNATIVE JUMP METHOD
 * 
 *     //float gravityScale = 1f;
 *     //float globalGravity = -9.81f;
 * 
 * Vector3 gravity = globalGravity * gravityScale * Vector3.up;
if (rb.velocity.y < 0)
{
    rb.AddForce(gravity * fallMultiplier, ForceMode.Acceleration);
}
else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
{
    rb.AddForce(gravity * lowJumpMultiplier, ForceMode.Acceleration);
}
else
{
    rb.AddForce(gravity, ForceMode.Acceleration);
}*/

