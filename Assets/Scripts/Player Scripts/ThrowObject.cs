using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shapes;

public class ThrowObject : MonoBehaviour
{
    public static event System.Action IsThrowing;
    public static event System.Action FinishedThrowing;
    public static event System.Action ObjectLanded;
    public static event System.Action ChildLanded;

    [SerializeField] Animator playerAnimator;

    //throw parameters
    [SerializeField] Camera gameCamera;
    [SerializeField] Transform throwTarget;
    [SerializeField] GameObject childSoundRadius;
    [SerializeField] GameObject objectSoundRadius;
    [SerializeField] float firingAngle = 45.0f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float chargeTime = 0.5f;
    [SerializeField] Disc chargeHUD;
    [SerializeField] Disc chargeHUDBase;
    float charging;

    [SerializeField] GameObject ammoOne;
    [SerializeField] GameObject ammoTwo;
    [SerializeField] GameObject ammoThree;
    [SerializeField] GameObject ammoFour;

    float ammoCount;

    public static bool inAir;

    //projectile transforms and ammo
    [SerializeField] Transform projectile;
    [SerializeField] private List<Transform> projectilesAmmo = new List<Transform>();
    Transform playerTransform;
    Transform childCube;

    //arc variables
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] Transform point3;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int vertexCount = 12;
    Vector3 lineRenderMidPoint;

    //set number of stationary guards in level
    //[SerializeField] StationaryGuard[] stationaryGuardScript;
    [SerializeField] List<StationaryGuard> stationaryGuardScript = new List<StationaryGuard>();

    bool isActive;

    bool throwTargetIsSet;
    bool triggerPressed;
    bool mousePressed;
    bool hasChild;
    bool childThrown;

    [SerializeField] SphereCollider targetSphereCollider;

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    void Awake()
    {
        playerTransform = transform;

        foreach(var stationaryGuard in FindObjectsOfType<StationaryGuard>())
        {
            stationaryGuardScript.Add(stationaryGuard);
        }
    }

    private void Update()
    {
        if (projectilesAmmo.Count > 0 || hasChild)
        {
            if(!inAir && isActive)
            {
                if (!triggerPressed)
                    LaunchProjectileMouse();
                if (!mousePressed)
                    LaunchProjectileController();
            }

            else if (Input.GetButton("ThrowMouse") | (Input.GetAxis("ThrowTrigger")) > 0 && inAir && isActive)
            {
                playerAnimator.SetTrigger("AlreadyThrowing");
            }
        }       
        
        else if (Input.GetButton("ThrowMouse") | (Input.GetAxis("ThrowTrigger")) > 0  && projectilesAmmo.Count == 0 && isActive)
        {
            playerAnimator.SetTrigger("NothingToThrow");
        }
    }

    private void OnCollisionEnter(Collision pickUp) //detects ammo collection
    {
        if (pickUp.gameObject.CompareTag("ammo")) 
        {
            projectilesAmmo.Add(pickUp.transform);
            pickUp.gameObject.SetActive(false);
            IncreaseAmmo();
        }

        if(pickUp.gameObject.CompareTag("ChildCube"))
        {
            hasChild = true;
            childCube = pickUp.transform;
            childCube.position = transform.position + new Vector3(0, 0.75f, 0);
            childCube.rotation = Quaternion.Euler(0, 0, 0);
            childCube.parent = transform;
        }
    }

    void LaunchProjectileMouse() //throws object capable of distracting guards
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetButton("ThrowMouse") && Physics.Raycast(ray, out hitInfo))
        {
            throwTarget.position = hitInfo.point;  //set target to mouse position using raycast
            ProjectileArcRender();
            charging += Time.deltaTime;
            ChargeHUDRender();
            mousePressed = true;
            
            if (IsThrowing != null)
                IsThrowing(); //disable player movement while throwing
        }

        if (Input.GetButtonUp("ThrowMouse") && charging >= chargeTime && mousePressed)
        {
            lineRenderer.gameObject.SetActive(false);
            childSoundRadius.SetActive(false);
            objectSoundRadius.SetActive(false);
            ResetHUDRender();
            charging = 0;
            
            if (!hasChild)
                DecreaseAmmo();

            StartCoroutine(SimulateProjectile());
            throwTargetIsSet = false;
            mousePressed = false;

            if (FinishedThrowing != null)
                FinishedThrowing(); //enable player movement when finished throwing
        }

        else if (Input.GetButtonUp("ThrowMouse") && charging < chargeTime && mousePressed)
        {
            lineRenderer.gameObject.SetActive(false);
            childSoundRadius.SetActive(false);
            objectSoundRadius.SetActive(false);
            ResetHUDRender();
            throwTarget.gameObject.SetActive(false);
            charging = 0;
            throwTargetIsSet = false;
            mousePressed = false;

            if (FinishedThrowing != null)
                FinishedThrowing(); //enable player movement when finished throwing
        }
    }

    void LaunchProjectileController()
    {
        if (Input.GetAxis("ThrowTrigger") > 0)
        {
            if (!throwTargetIsSet)
            {
                targetSphereCollider.enabled = true;
                var targetPosition = ((transform.position + Vector3.zero) / 2 ) + new Vector3(0, (transform.position.y + 0)/2 - 0.5f, 0);
                throwTarget.position = targetPosition;
                throwTargetIsSet = true;
            }

            TargetMovement();

            ProjectileArcRender();
            charging += Time.deltaTime;
            ChargeHUDRender();
            triggerPressed = true;

            if (IsThrowing != null)
                IsThrowing();
        }

        if (Input.GetAxis("ThrowTrigger") == 0 && charging >= chargeTime && triggerPressed)
        {
            lineRenderer.gameObject.SetActive(false);
            childSoundRadius.SetActive(false);
            objectSoundRadius.SetActive(false);
            ResetHUDRender();
            charging = 0;

            if (!hasChild)
                DecreaseAmmo();

            StartCoroutine(SimulateProjectile());
            throwTargetIsSet = false;
            targetSphereCollider.enabled = false;
            triggerPressed = false;

            if (FinishedThrowing != null)
                FinishedThrowing(); //enable player movement when finished throwing
        }

        else if (Input.GetAxis("ThrowTrigger") == 0 && charging < chargeTime && triggerPressed)
        {
            lineRenderer.gameObject.SetActive(false);
            childSoundRadius.SetActive(false);
            objectSoundRadius.SetActive(false);
            ResetHUDRender();
            throwTarget.gameObject.SetActive(false);
            charging = 0;
            throwTargetIsSet = false;
            targetSphereCollider.enabled = false;
            triggerPressed = false;

            if (FinishedThrowing != null)
                FinishedThrowing(); //enable player movement when finished throwing
        }
    }

    void ProjectileArcRender() //renders arc so player can see where projectile will land
    {
        float targetDistance = Vector3.Distance(playerTransform.position, throwTarget.position);
        float halfX = playerTransform.position.x + (throwTarget.position.x - playerTransform.position.x) / 2;
        float halfZ = playerTransform.position.z + (throwTarget.position.z - playerTransform.position.z) / 2;

        if(targetDistance > 5)
            point2.position = new Vector3(halfX, targetDistance/1.4f, halfZ);
        else
            point2.position = new Vector3(halfX, 3.7f, halfZ);

        lineRenderer.gameObject.SetActive(true);
        throwTarget.gameObject.SetActive(true);

        if(hasChild)
        {
            childSoundRadius.SetActive(true);
        }
        else if(!hasChild)
        {
            objectSoundRadius.SetActive(true);
        }

        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position + new Vector3(0, 0.5f, 0), point2.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }
    IEnumerator SimulateProjectile() //simulates projectile by moving it along an arc to target destination
    {
        if(hasChild)
        {
            projectile = childCube;
            childCube.parent = null;
            childCube.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            hasChild = false;
            childThrown = true;
        }
        
        else if(!hasChild)
        {
            projectile = projectilesAmmo[0];
            projectilesAmmo[0].gameObject.SetActive(true);
            projectilesAmmo.Remove(projectilesAmmo[0]);
        }

        // Move projectile to the position of throwing object + add some offset if needed.
        projectile.position = playerTransform.position + new Vector3(0, 1, 0);

        // Calculate distance to target
        float target_Distance = Vector3.Distance(projectile.position, throwTarget.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        projectile.rotation = Quaternion.LookRotation(throwTarget.position - projectile.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            inAir = true; //prevent player from throwing another object while one is already in the air

            projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        inAir = false; //allow player to throw again
        throwTarget.gameObject.SetActive(false);
        Vector3 newTarget = throwTarget.position;

        if (!childThrown && stationaryGuardScript.Count > 0)
        {
            foreach (StationaryGuard script in stationaryGuardScript)
            {
                script.Target = newTarget;
            }
        }

        else if (childThrown && stationaryGuardScript.Count > 0)
        {
            foreach (StationaryGuard script in stationaryGuardScript)
            {
                script.ChildTarget = newTarget;
            }
        }

        if (childThrown)
        {
            ChildLanded();
            childThrown = false;
        }

        else if (!childThrown)
        {
            ObjectLanded();
        }
    }

    void ChargeHUDRender() //change the parameters to variables?
    {
        chargeHUDBase.AngRadiansStart = Mathf.Deg2Rad * 22;
        chargeHUDBase.AngRadiansStart = Mathf.Deg2Rad * 158;
        
        if(chargeHUD.AngRadiansStart > (Mathf.Deg2Rad * 25))
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

    void IncreaseAmmo()
    {
        ammoCount++;

        switch (ammoCount)
        {
            case 1:
                ammoOne.SetActive(true);
                break;
            case 2:
                ammoTwo.SetActive(true);
                break;
            case 3:
                ammoThree.SetActive(true);
                break;
            case 4:
                ammoFour.SetActive(true);
                break;
            default:
                break;
        }
    }

    void DecreaseAmmo()
    {
        switch (ammoCount)
        {
            case 1:
                ammoOne.SetActive(false);
                break;
            case 2:
                ammoTwo.SetActive(false);
                break;
            case 3:
                ammoThree.SetActive(false);
                break;
            case 4:
                ammoFour.SetActive(false);
                break;
            default:
                break;
        }

        ammoCount--;
    }

    public void RemoveGuardOnDestroy(StationaryGuard stationaryGuard)
    {
        stationaryGuardScript.Remove(stationaryGuard);
    }

    void TargetMovement()
    {
        var inputDirection = new Vector3(Input.GetAxisRaw("RightJoystickX"), 0, Input.GetAxisRaw("RightJoystickY")).normalized;
        var rotatedDirection = Quaternion.Euler(0, 45, 0) * inputDirection;

        float inputMagnitude = inputDirection.magnitude;

        float targetAngle = Mathf.Atan2(rotatedDirection.x, rotatedDirection.z) * Mathf.Rad2Deg;

        throwTarget.rotation = Quaternion.Euler(Vector3.up * targetAngle);
        throwTarget.position += throwTarget.forward * inputMagnitude * 12 * Time.deltaTime;
    }
}
