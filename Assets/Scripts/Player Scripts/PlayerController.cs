using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class PlayerController : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;

    [SerializeField] Animator playerAnimator;
    [SerializeField] ParticleSystem selfDestruct;
    [SerializeField] GameObject playerHUD;
    [SerializeField] public GameObject interactButtonIcon;

    //player movement parameters
    [SerializeField] float moveSpeed = 7;
    [SerializeField] float smoothMoveTime = 0.1f;
    [SerializeField] float turnSpeed = 8;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    float inputMagnitude;
    Vector3 velocity;
    Vector3 rotatedDirection;
    Vector3 inputDirection;

    Rigidbody rb;
    bool disabled;
    bool isMoving;

    ChangeColor changeColorScript;
    public bool IsMoving
    {
        get { return isMoving; }
    }

    Transform playerTransform;
    public Transform PlayerTransform
    {
        get { return playerTransform; }
        set { playerTransform = value;  }
    }

    float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        PlayerDetection.OnGameLose += HandleOnGameLose;
        FieldOfView.OnGameLose += HandleOnGameLose;
        Drones.OnGameLose += HandleOnGameLose;
        SecurityCameras.OnGameLose += HandleOnGameLose;
        GuardDog.OnGameLose += HandleOnGameLose;
        LaserGuard.OnGameLose += HandleOnGameLose;
        FallTrigger.OnGameLose += HandleOnGameLose;

        LevelFinish.OnGameWin += Disable;
        
        ThrowObject.IsThrowing += Disable;
        ThrowObject.FinishedThrowing += Enable;

        PushObject.OnPushStart += Disable;
        PushObject.OnPushEnd += Enable;

        PushChild.IsPushed += Disable;
        PushChild.HitObject += Enable;

        DialogueManager.OnEnterDialogue += Disable;
        DialogueManager.OnExitDialogue += Enable;

        changeColorScript = GetComponent<ChangeColor>();
    }

    void Update()
    {
        MovementInput();

        if(Input.GetButton("SelfDestruct") && GameManager.Instance.CurrentGameState != GameManager.GameState.GAMELOSE)
        {
            SelfDestruct();
        }
        else if(Input.GetButtonUp("SelfDestruct") && GameManager.Instance.CurrentGameState != GameManager.GameState.GAMELOSE)
        {
            disabled = false;
            timer = 0;
            playerAnimator.SetBool("SelfDestruct", false);
        }
    }

    private void FixedUpdate()
    {
        RigidBodyMovement();
    }

    void MovementInput()
    {
        rotatedDirection = Vector3.zero; //disable player movement on Gameover
        if(!disabled) 
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            rotatedDirection = Quaternion.Euler(0, 45, 0) * inputDirection; //rotated for isometric view
        }

        inputMagnitude = rotatedDirection.magnitude; //get magnitude so that player isnt moving/turning without input (multiplying by 0 magnitude)
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime); //smooths player movement

        float targetAngle = Mathf.Atan2(rotatedDirection.x, rotatedDirection.z) * Mathf.Rad2Deg; //calculate the target angle and convert to degrees
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude); //smooths player turning, and stores for RigidBodyMovement(), prevents rotation from resetting to zero

        velocity = transform.forward * moveSpeed * smoothInputMagnitude; //store data in Vector3 for RigidBodyMovement()

        if (inputMagnitude == 0)
        {
            isMoving = false;
            playerAnimator.SetBool("IsMoving", false);
        }

        else
        {
            isMoving = true;
            playerAnimator.SetBool("IsMoving", true);
        }
    }

    void RigidBodyMovement() //in separate method so physics can be run through fixedupdate
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    public void Disable()
    {
        disabled = true;
    }
    public void Enable()
    {
        disabled = false;
    }

    void SelfDestruct()
    {
        disabled = true;
        timer += Time.deltaTime;
        playerAnimator.SetBool("SelfDestruct", true);

        if (timer >= 1.45f)
        {
            selfDestruct.Play();
            OnGameLose("SelfDestruct");
        }
    }

    private void PlayDissolve()
    {
        playerAnimator.StopPlayback();
        playerAnimator.SetTrigger("Dead");

        if(changeColorScript.CurrentPlayerColor != ChangeColor.PlayerColor.BLUE)
        {
            Destroy(gameObject.GetComponent<BoxCollider>(), 0.6f);
            Destroy(gameObject.GetComponent<MeshRenderer>(), 0.6f);
            Destroy(gameObject.GetComponentInChildren<Rectangle>(), 0.6f);
        }
        else if(changeColorScript.CurrentPlayerColor == ChangeColor.PlayerColor.BLUE)
        {
            Destroy(gameObject.GetComponent<SphereCollider>(), 0.6f);
            Destroy(gameObject.GetComponent<MeshRenderer>(), 0.6f);
            Destroy(gameObject.GetComponentInChildren<Rectangle>(), 0.6f);
        }

    }

    void HandleOnGameLose(string gameOverSource)
    {
        Disable();
        if (gameOverSource == "Laser")
        {
            PlayDissolve();
            playerHUD.SetActive(false);
        }
            
    }

    private void OnDestroy()
    {
        PlayerDetection.OnGameLose -= HandleOnGameLose;
        FieldOfView.OnGameLose -= HandleOnGameLose;
        Drones.OnGameLose -= HandleOnGameLose;
        SecurityCameras.OnGameLose -= HandleOnGameLose;
        GuardDog.OnGameLose -= HandleOnGameLose;
        LaserGuard.OnGameLose -= HandleOnGameLose;
        FallTrigger.OnGameLose -= HandleOnGameLose;

        LevelFinish.OnGameWin -= Disable;

        ThrowObject.IsThrowing -= Disable;
        ThrowObject.FinishedThrowing -= Enable;

        PushObject.OnPushStart -= Disable;
        PushObject.OnPushEnd -= Enable;

        PushChild.IsPushed -= Disable;
        PushChild.HitObject -= Enable;

        DialogueManager.OnEnterDialogue -= Disable;
        DialogueManager.OnExitDialogue -= Enable;
    }
}
