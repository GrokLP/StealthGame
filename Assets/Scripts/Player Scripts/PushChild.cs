using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class PushChild : MonoBehaviour
{
    public static event System.Action IsPushed;
    public static event System.Action HitObject;

    [SerializeField] GameObject grid;

    [SerializeField] ParticleSystem dust;

    float collisionDistance = 0.15f;
    [SerializeField] float distance;
    [SerializeField] LayerMask grabMask;
    [SerializeField] LayerMask collisionMask;

    bool pushedAway;
    bool startedGrab;
    Vector3 pushDirection;
    PushedChild pushedChildScript;
    Transform pushedChildPosition;

    [SerializeField] float chargeTime = 1f;
    [SerializeField] Disc chargeHUD;
    [SerializeField] Disc chargeHUDBase;
    [SerializeField] Color chargeHUDColor;
    float charging;

    bool isActive;
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("PushChildCube") != null)
        {
            pushedChildScript = GameObject.FindGameObjectWithTag("PushChildCube").GetComponent<PushedChild>();
            pushedChildPosition = GameObject.FindGameObjectWithTag("PushChildCube").GetComponent<Transform>();
        }
    }

    private void Update()
    {
        if(Input.GetButton("ShowGridKey") | (Input.GetAxis("ShowGridController") > 0))
        {
            grid.SetActive(true);
        }
        
        if(Input.GetButtonUp("ShowGridKey") | (Input.GetAxis("ShowGridController") == 0))
        {
            grid.SetActive(false);
        }
        
        RaycastHit hit;
        if (Input.GetButton("Push") && Physics.Raycast(transform.position, transform.forward, out hit, distance, grabMask) && isActive)
        {
            StartGrab(hit);
        }

        if (Input.GetButtonUp("Push") && startedGrab && charging >= chargeTime)
        {
            pushedChildScript.animator.SetBool("FrontPush", false);
            pushedChildScript.animator.SetBool("SidePush", false);
            GetDirection();
            dust.Play();
            pushedAway = true;
            charging = 0;
            ResetHUDRender();

            pushedChildScript.GetDirection();
            pushedChildScript.ChildPushedAway = true;
        }

        else if (Input.GetButtonUp("Push") && startedGrab && charging < chargeTime)
        {
            pushedAway = false;
            pushedChildScript.animator.SetBool("FrontPush", false);
            pushedChildScript.animator.SetBool("SidePush", false);
            charging = 0;
            ResetHUDRender();
            if (HitObject != null)
                HitObject();
        }



        //Debug.DrawRay(transform.position + new Vector3(0.5f, 0, -0.5f), -transform.forward * collisionDistance, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(-0.5f, 0, -0.5f), -transform.forward * collisionDistance, Color.red);
    }

    private void FixedUpdate()
    {
        if (pushedAway == true)
            PushedAway();
    }

    void StartGrab(RaycastHit hit)
    {
        if (hit.collider.CompareTag("PushChildCube"))
        {
            if (IsPushed != null)
            {
                IsPushed();
            }

            SnapToPushChild();
            startedGrab = true;
            charging += Time.deltaTime;

            ChargeHUDRender();
        }
    }

    void PushedAway()
    {
        startedGrab = false;
        transform.position += pushDirection * 10 * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 0, -0.5f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-0.5f, 0, -0.5f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(0.5f, 0, 0.5f), pushDirection, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-0.5f, 0, 0.5f), pushDirection, out hit, collisionDistance, collisionMask))
        {
            pushedAway = false;

            if (HitObject != null)
                HitObject();

            CameraShake.Instance.StartShake(0.1f, 0.1f);
            dust.Stop();
        }
    }

    public void GetDirection()
    {
        pushDirection = new Vector3(transform.position.x - pushedChildPosition.position.x, 0, transform.position.z - pushedChildPosition.position.z);
    }

    void ChargeHUDRender()
    {
        chargeHUD.Color = chargeHUDColor;

        chargeHUDBase.AngRadiansStart = Mathf.Deg2Rad * 22;
        chargeHUDBase.AngRadiansStart = Mathf.Deg2Rad * 158;

        if (chargeHUD.AngRadiansStart > (Mathf.Deg2Rad * 25))
        {
            var num = (charging / chargeTime) * 130;
            chargeHUD.AngRadiansStart = Mathf.Deg2Rad * (155 - num);
        }
    }

    void ResetHUDRender()
    {
        chargeHUDBase.AngRadiansStart = Mathf.Deg2Rad * 22;
        chargeHUDBase.AngRadiansStart = Mathf.Deg2Rad * 22;

        chargeHUD.AngRadiansStart = Mathf.Deg2Rad * 155;
    }

    void SnapToPushChild()
    {
        float[] snapDisctances = new float[4];

        snapDisctances[0] = Vector3.Distance(transform.position, pushedChildScript.forward.position);
        snapDisctances[1] = Vector3.Distance(transform.position, pushedChildScript.back.position);
        snapDisctances[2] = Vector3.Distance(transform.position, pushedChildScript.right.position);
        snapDisctances[3] = Vector3.Distance(transform.position, pushedChildScript.left.position);

        int snapTarget = GetIndexOfLowestValue(snapDisctances);

        switch (snapTarget) // currently lifts player off ground...but might be fun to keep?
        {
            case 0:
                transform.position = pushedChildScript.forward.position;
                pushedChildScript.animator.SetBool("FrontPush", true);
                break;
            case 1:
                transform.position = pushedChildScript.back.position;
                pushedChildScript.animator.SetBool("FrontPush", true);
                break;
            case 2:
                transform.position = pushedChildScript.right.position;
                pushedChildScript.animator.SetBool("SidePush", true);
                break;
            case 3:
                transform.position = pushedChildScript.left.position;
                pushedChildScript.animator.SetBool("SidePush", true);
                break;
            default:
                Debug.Log("snap error");
                break;
        }
    }

    public int GetIndexOfLowestValue(float[] snapDistances)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < snapDistances.Length; i++)
        {
            if (snapDistances[i] < value)
            {
                index = i;
                value = snapDistances[i];
            }
        }
        return index;
    }
}
