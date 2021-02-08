using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class PushChild : MonoBehaviour
{
    public static event System.Action IsPushed;
    public static event System.Action HitObject;

    float collisionDistance = 0.1f;
    [SerializeField] float distance;
    [SerializeField] LayerMask grabMask;
    [SerializeField] LayerMask collisionMask;

    bool pushedAway;
    bool startedGrab;
    PlayerController playerController;

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

    PushedChild pushedChildScript;

    private void Start()
    {
        playerController = GetComponent<PlayerController>(); //can serialize this when done
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            SnapToPushChild();
        
        RaycastHit hit;
        if (Input.GetButton("Push") && Physics.Raycast(transform.position, transform.forward, out hit, distance, grabMask) && isActive)
        {
            StartGrab(hit);
        }

        if (Input.GetButtonUp("Push") && startedGrab && charging >= chargeTime)
        {
            pushedAway = true;
            charging = 0;
            ResetHUDRender();

            pushedChildScript.PushDirection = transform.TransformDirection(Vector3.forward);
            pushedChildScript.ChildPushedAway = true;
        }

        else if (Input.GetButtonUp("Push") && startedGrab && charging < chargeTime)
        {
            pushedAway = false;
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
            pushedChildScript = hit.collider.GetComponent<PushedChild>();

            if (IsPushed != null)
            {
                IsPushed();
            }
            
            startedGrab = true;
            charging += Time.deltaTime;

            ChargeHUDRender();
        }
    }

    void PushedAway()
    {
        startedGrab = false;
        transform.position += -transform.forward * 10 * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0.5f, 0, -0.5f), -transform.forward, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-0.5f, 0, -0.5f), -transform.forward, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(0.5f, 0, 0.5f), -transform.forward, out hit, collisionDistance, collisionMask) |
            Physics.Raycast(transform.position + new Vector3(-0.5f, 0, 0.5f), -transform.forward, out hit, collisionDistance, collisionMask))
        {
            pushedAway = false;

            if (HitObject != null)
                HitObject();
        }
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
        if(transform.rotation.y >= 0 && transform.rotation.y < 45)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if(transform.rotation.y >= 45 && transform.rotation.y < 90)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else if (transform.rotation.y >= 90 && transform.rotation.y < 135)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else if (transform.rotation.y >= 135 && transform.rotation.y < 180)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.rotation.y >= 180 && transform.rotation.y < 225)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.rotation.y >= 225 && transform.rotation.y < 270)
        {
            transform.localEulerAngles = new Vector3(0, 270, 0);
        }
        else if (transform.rotation.y >= 270 && transform.rotation.y < 315)
        {
            transform.localEulerAngles = new Vector3(0, 270, 0);
        }
        else if (transform.rotation.y >= 315 && transform.rotation.y < 360)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
