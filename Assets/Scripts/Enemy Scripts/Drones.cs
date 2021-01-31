using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class Drones : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;

    //Guard detection parameters
    [SerializeField] LayerMask viewMask;

    //[SerializeField] Light spotLight;
    [SerializeField] VolumetricLightBeam spotLight;
    [SerializeField] float viewDistance;
    [SerializeField] float tooCloseDistance;
    [SerializeField] float timeToSpotPlayer = 0.1f;
    float playerVisibleTimer;
    [SerializeField] float viewAngle; //view angle is set by spotlight 

    Color originalSpotLightColor;

    ChangeColor.PlayerColor currentPlayerColor;

    Transform player;
    PlayerVisibility playerVisibility;

    bool gameWin;

    [SerializeField] float speed = 5;
    [SerializeField] float waitTime = 0.3f;

    [SerializeField] Transform pathHolder;

    void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(UpdateSpotLightColor);
        GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChange);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerVisibility = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVisibility>();

        //viewAngle = spotLight.spotAngle;
        originalSpotLightColor = spotLight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); //setting waypoints to gaurd's y so that drone stays at same level in air
        }

        StartCoroutine(GuardPatrol(waypoints));
    }

    private void Update()
    {
        if (!gameWin)
        {
            IsDetected();
            TooClose();
        }
    }

    bool DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance && playerVisibility.IsVisible()) //check if distance to player is within view range
        {
            Vector3 detectPlayerDirection = (player.transform.position - transform.position).normalized; //get look direction
            float detectPlayerAngle = Vector3.Angle(transform.forward, detectPlayerDirection); //get angle and then check if that angle is within half of view angle (half of view angle from look forward centre point)
            if (detectPlayerAngle < (viewAngle / 2))
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask)) //linecast because already checked if player is in range, checks if anything is blocking line of sight
                    return true;
            }
        }
        return false;
    }

    void IsDetected()
    {
        if (DetectPlayer())
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
                spotLight.color = Color.Lerp(originalSpotLightColor, Color.green, playerVisibleTimer / timeToSpotPlayer);
                break;
        }

        if (playerVisibleTimer >= timeToSpotPlayer)
            if (OnGameLose != null)
            {
                OnGameLose("Spotlight");
                StopAllCoroutines();
            }
    }
    void TooClose()
    {
        if (Vector3.Distance(transform.position, player.position) < tooCloseDistance) //create small circle around guard that player cannot enter
        {
            if (OnGameLose != null)
            {
                OnGameLose("TooClose");
            }
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

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition); //completes loop   
    }

    IEnumerator GuardPatrol(Vector3[] waypoints)
    {
        //transform.position = waypoints[0];

        int targetWaypointsIndex = 0;
        Vector3 targetWaypoint = waypoints[targetWaypointsIndex];
        
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointsIndex = (targetWaypointsIndex + 1) % waypoints.Length; //if index reaches array length it's reset to 0 (because remainder is 0)
                targetWaypoint = waypoints[targetWaypointsIndex]; //set next target
                yield return new WaitForSeconds(waitTime);
            }
            yield return null; //ensures it only travels max distance per frame
        }
    }


}
