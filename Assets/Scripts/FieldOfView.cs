using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public static event System.Action<string> OnGameLose;


    public float viewRadius;
    [Range (0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistanceThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    [SerializeField] Renderer rend;

    [SerializeField] Material yellow;
    [SerializeField] Material white;
    [SerializeField] Material red;
    [SerializeField] Material blue;
    [SerializeField] Material green;

    ChangeColor.PlayerColor currentPlayerColor;

    [SerializeField] float tooCloseDistance;
    Transform player;
    PlayerVisibility playerVisibility;

    bool gameWin;

    private void Start()
    {
        ChangeColor.Instance.OnPlayerColorChange.AddListener(UpdateSpotLightColor);
        GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChange);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerVisibility = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerVisibility>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        rend.material = yellow;
        
        //StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    private void Update()
    {
        if(!gameWin)
            FindVisibleTargets();//was tooclose()
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); //right now means player can be detected at dif heights

        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            if(Vector3.Distance(transform.position, target.position) < tooCloseDistance)
            {
                if (OnGameLose != null)
                {
                    OnGameLose("TooClose");
                }
            }

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, directionToTarget) < viewAngle/2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask) && playerVisibility.IsVisible())
                {
                    IsDetected();
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewpoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            
            if(i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                
                if(oldViewCast.hit != newViewCast.hit || oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewpoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewpoints.Add(edge.pointB);
                    }
                }
            }
            
            viewpoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewpoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = new Vector3(0, 0, 0);//verctor3.zero
        for(int i = 0; i < vertexCount -1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewpoints[i]);

            if(i < vertexCount-2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    void IsDetected()
    {
        //maybe do gamewin check here?
        switch (currentPlayerColor) //need to figure out color change on detection
        {
            case ChangeColor.PlayerColor.WHITE:

                rend.material = white;
                break;

            case ChangeColor.PlayerColor.RED:

                rend.material = red;
                break;

            case ChangeColor.PlayerColor.BLUE:

                rend.material = blue;
                break;

            case ChangeColor.PlayerColor.GREEN:

                rend.material = green;
                break;
        }

        if (OnGameLose != null)
        {
            OnGameLose("Spotlight");
            if (GetComponent<WaypointGuard>() != null)
                GetComponent<WaypointGuard>().StopCoroutines();
            else if (GetComponent<StationaryGuard>() != null)
                GetComponent<StationaryGuard>().StopCoroutines();
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
