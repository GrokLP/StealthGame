using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserGuard : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;
    
    private NavMeshAgent shootingGuard;
    [SerializeField] float laserRange;
    [SerializeField] float tooCloseDistance;
    [SerializeField] bool laserImmune;

    Vector3 destination;
    LineRenderer laserLine;
    Vector3 rayOrigin;

    LaserTrigger laserTriggerScript;

    ColorLaserGuard colorLaserGuardScript;

    Transform player;

    bool gameWin;
    bool laserKill;
    bool laserTriggerActivatedSound;
    bool laserTriggerDeactivatedSound;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChange);

        colorLaserGuardScript = GetComponent<ColorLaserGuard>();

        laserLine = GetComponent<LineRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!gameWin)
            TooClose();

        if (colorLaserGuardScript.IsActive)
        {
            rayOrigin = transform.position + new Vector3(0, -0.5f, 0);

            RaycastHit hitInfo;

            laserLine.SetPosition(0, transform.position + new Vector3(0, -0.5f, 0));

            if (Physics.Raycast(rayOrigin, transform.forward, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Player") && !gameWin)
                {
                    laserLine.SetPosition(1, hitInfo.point);
                    if (OnGameLose != null)
                    {
                        OnGameLose("Laser");
                        if(!laserKill)
                        {
                            AudioManager.Instance.PlaySound("LaserBeam");
                            laserKill = true;
                        }

                    }
                }
                else if (hitInfo.collider.CompareTag("PushChildCube") && !gameWin) 
                {
                    laserLine.SetPosition(1, hitInfo.point);
                    //hitInfo.collider.GetComponent<Animator>().SetTrigger("Dead"); Need to add dissolve
                    Destroy(hitInfo.collider.gameObject, 0.8f);
                    if (OnGameLose != null)
                    {
                        OnGameLose("ChildLaser");
                        if (!laserKill)
                        {
                            AudioManager.Instance.PlaySound("LaserBeam");
                            laserKill = true;
                        }
                    }
                }
                else if (hitInfo.collider.CompareTag("ChildCube") && !gameWin)
                {
                    laserLine.SetPosition(1, hitInfo.point);
                    hitInfo.collider.GetComponent<Animator>().SetTrigger("Dead");
                    Destroy(hitInfo.collider.gameObject, 0.8f);
                    if (OnGameLose != null)
                    {
                        OnGameLose("ChildLaser");
                        if (!laserKill)
                        {
                            AudioManager.Instance.PlaySound("LaserBeam");
                            laserKill = true;
                        }
                    }
                }

                else if (hitInfo.collider.CompareTag("Enemy") && !laserImmune)
                {
                    laserLine.SetPosition(1, hitInfo.point);
                    hitInfo.collider.GetComponentInParent<Animator>().SetTrigger("Dead");
                    Destroy(hitInfo.collider.transform.parent.gameObject, 0.8f);
                    if (!laserKill)
                    {
                        AudioManager.Instance.PlaySound("LaserBeam");
                        laserKill = true;
                    }
                }

                else if (hitInfo.collider.CompareTag("LaserTrigger"))
                {
                    laserTriggerScript = hitInfo.collider.GetComponent<LaserTrigger>(); //this is prob not performant?
                    laserTriggerScript.Trigger = true;
                    laserLine.SetPosition(1, hitInfo.point);
                    if(!laserTriggerActivatedSound)
                    {
                        AudioManager.Instance.PlaySound("TriggerActivated");
                        laserTriggerActivatedSound = true;
                    }
                    laserTriggerDeactivatedSound = false;
                }

                else if (hitInfo.collider)
                {
                    laserLine.SetPosition(1, hitInfo.point);
                    if (laserTriggerScript != null)
                    {
                        laserTriggerScript.Trigger = false;
                        laserTriggerActivatedSound = false;

                        if(!laserTriggerDeactivatedSound)
                        {
                            AudioManager.Instance.PlaySound("TriggerDeactivated");
                            laserTriggerDeactivatedSound = true;
                        }
                    }
                }
            }

            else
            {
                laserLine.SetPosition(1, transform.forward * laserRange);
                if (laserTriggerScript != null)
                    laserTriggerScript.Trigger = false;
            }
        }

    }

    void OnGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (currentState == GameManager.GameState.GAMEWIN)
            gameWin = true;
    }

    void TooClose()
    {
        if (Vector3.Distance(transform.position, player.position) < tooCloseDistance) //create small circle around guard that player cannot enter
        {
            if (OnGameLose != null)
            {
                OnGameLose("Laser");
                if (!laserKill)
                {
                    AudioManager.Instance.PlaySound("LaserBeam");
                    laserKill = true;
                }
            }
        }
    }
}

