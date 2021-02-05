using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class SecurityCameras : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;

    [SerializeField] float viewDistance;
    [SerializeField] float timeToSpotPlayer = 0.5f;

    [SerializeField] VolumetricLightBeam spotLight;
    [SerializeField] LayerMask viewMask;
    [SerializeField] Color red;

    float viewAngle;
    float playerVisibleTimer;

    Color originalSpotLightColor;
    Transform player;
    Transform childCube;
    bool childPresent;
    PlayerVisibility playerVisibility;

    ChangeColor.PlayerColor currentPlayerColor;

    bool gameWin;

    void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(UpdateSpotLightColor);
        GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChange);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerVisibility = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVisibility>();

        if (GameObject.FindGameObjectWithTag("ChildCube") != null)
        {
            childCube = GameObject.FindGameObjectWithTag("ChildCube").transform;
            childPresent = true;
        }

        viewAngle = spotLight.spotAngle;
        originalSpotLightColor = spotLight.color;

    }
    
    void Update()
    {
        if(gameWin == false)
            IsDetected();
    }

    bool PlayerDetection()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance && playerVisibility.IsVisible()) //check if distance to player is within view range
        {
            Vector3 detectPlayerDirection = (player.transform.position - transform.position).normalized; //get look direction
            float detectPlayerAngle = Vector3.Angle(transform.forward * -1, detectPlayerDirection); //get angle and then check if that angle is within half of view angle (half of view angle from look forward centre point)
            if (detectPlayerAngle < (viewAngle / 2))
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask)) //linecast because already checked if player is in range, checks if anything is blocking line of sight
                    return true;
            }
        }

        else if (childPresent)
        {
            if (Vector3.Distance(transform.position, childCube.position) < viewDistance) //check if distance to player is within view range
            {
                Vector3 detectChildCubeDirection = (childCube.transform.position - transform.position).normalized; //get look direction
                float detectChildCubeAngle = Vector3.Angle(transform.forward, detectChildCubeDirection); //get angle and then check if that angle is within half of view angle (half of view angle from look forward centre point)
                if (detectChildCubeAngle < (viewAngle / 2))
                {
                    if (!Physics.Linecast(transform.position, childCube.position, viewMask)) //linecast because already checked if player is in range, checks if anything is blocking line of sight
                        return true;
                }
            }
        }

        return false;
    }
    void IsDetected()
    {
        if (PlayerDetection())
            playerVisibleTimer += Time.deltaTime;
        else
            playerVisibleTimer -= Time.deltaTime;

        switch (currentPlayerColor)
        {
            case ChangeColor.PlayerColor.WHITE:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
                spotLight.color = Color.Lerp(originalSpotLightColor, new Color(1, 1, 1, 0.5f), playerVisibleTimer / timeToSpotPlayer);
                break;

            case ChangeColor.PlayerColor.RED:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
                spotLight.color = Color.Lerp(originalSpotLightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);
                break;

            case ChangeColor.PlayerColor.BLUE:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
                spotLight.color = Color.Lerp(originalSpotLightColor, Color.blue, playerVisibleTimer / timeToSpotPlayer);
                break;

            case ChangeColor.PlayerColor.GREEN:
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
                spotLight.color = Color.Lerp(originalSpotLightColor, new Color(0, 1, 0, 0.5f), playerVisibleTimer / timeToSpotPlayer);
                break;
        }

        if (playerVisibleTimer >= timeToSpotPlayer)
            if (OnGameLose != null)
            {
                OnGameLose("Camera");
                StopAllCoroutines();
            }
    }

    void UpdateSpotLightColor(ChangeColor.PlayerColor currentColor, ChangeColor.PlayerColor previousColor)
    {
        currentPlayerColor = currentColor;
    }

    void OnGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (currentState == GameManager.GameState.GAMEWIN)
            gameWin = true;
    }
}
