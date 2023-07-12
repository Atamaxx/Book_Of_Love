using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public class PlatformController : MonoBehaviour
{
    [SerializeField] private BookOf.Time _timeLine;
    public List<GameObject> Waypoints = new();
    public GameObject WaypointParent;
    public Sprite WaypointSprite;
    public Color previewColor;

    #region Privare Variables
    private float _timeDistance;
    private int _waypointIndex = 0;
    private int _numberOfPoints = 0;
    private readonly List<Vector3> _waypoints = new();
    private float _wayLength;
    #endregion

    #region Transformations
    [BoxGroup("MOVE BY DISTANCE")] public bool toMoveByDistance;
    [BoxGroup("MOVE BY DISTANCE"), ShowIf("toMoveByDistance")]
    public MovingPlatform movingPlatform;

    [BoxGroup("CONSTANT MOVEMENT")] public bool toConstantMove;
    [BoxGroup("CONSTANT MOVEMENT"), ShowIf("toConstantMove")]
    public ConstantMovement constantMovement;

    [BoxGroup("ROTATION")] public bool toRotateByDistance;
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public RotatingPlatform rotatingPlatform;

    [BoxGroup("OVERLAPPING")] public bool toOverlap;
    [BoxGroup("OVERLAPPING"), ShowIf("toOverlap")]
    public Overlapping overlapping;

    [BoxGroup("100%")] public bool EndAction = false;
    [BoxGroup("100%"), ShowIf("EndAction")]
    public float DestroyDelay = 1.5f;
    #endregion

    private void Start()
    {
        AddScripts();

        g_platformBounds = GetComponent<BoxCollider2D>().bounds;
        _numberOfPoints = Waypoints.Count;

        for (int i = 0; i < _numberOfPoints; i++)
        {
            _waypoints.Add(Waypoints[i].transform.position);
        }

        if (movingPlatform != null && movingPlatform.toLoop)
        {
            _waypoints.Add(Waypoints[0].transform.position);
        }

        _wayLength = My.Line.CalculateLength(_waypoints);
    }

    private void AddScripts()
    {
        if (toMoveByDistance)
        {
            movingPlatform = GetComponent<MovingPlatform>();
        }
        else if (toConstantMove)
        {
            constantMovement = GetComponent<ConstantMovement>();
        }

        if (toRotateByDistance)
        {
            rotatingPlatform = GetComponent<RotatingPlatform>();
        }

        if (toOverlap)
        {
            overlapping = GetComponent<Overlapping>();
        }
    }

    void Update()
    {
        _timeDistance = _timeLine.ÑurrentLength;

        if (toOverlap && overlapping.IsOverlaping)
        {
            toConstantMove = true;
        }
        else toConstantMove = false;

        if (toMoveByDistance)
        {
            movingPlatform.MoveByDistance(_waypoints, _timeDistance, _wayLength);
        }
        else if (toConstantMove)
        {
            constantMovement.ConstantMove(_waypoints, ref _waypointIndex);
        }

        if (toRotateByDistance)
        {
            rotatingPlatform.RotateByDistance(_timeDistance);
        }

        
    }


    #region Editor

#if UNITY_EDITOR

    [Button]
    private void CreateWaypoint()
    {
        if (WaypointParent == null) return;
        GameObject waypoint = new("Waypoint_" + name + Waypoints.Count);
        SpriteRenderer spriteRen = waypoint.AddComponent<SpriteRenderer>();
        spriteRen.sprite = WaypointSprite;
        spriteRen.color = previewColor;
        waypoint.transform.parent = WaypointParent.transform;
        waypoint.transform.position = transform.position;
        Waypoints.Add(waypoint);
    }
    [Button("Add Parent's Waypoints")]
    private void AddParentWaypoints()
    {
        if (WaypointParent == null) return;
        for (int i = 0; i < WaypointParent.transform.childCount; i++)
        {
            Transform child = WaypointParent.transform.GetChild(i);
            Waypoints.Add(child.gameObject);
        }
    }
    [Button]
    private void DeleteLastWaypoint()
    {
        if (Waypoints.Count == 0 && WaypointParent == null) return;

        Waypoints.RemoveAt(Waypoints.Count - 1);

        int childCount = WaypointParent.transform.childCount;
        DestroyImmediate(WaypointParent.transform.GetChild(childCount - 1).gameObject);
    }

    [Button]
    private void DeleteWaypoints()
    {
        if (WaypointParent == null) return;

        Waypoints.Clear();
        int childCount = WaypointParent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(WaypointParent.transform.GetChild(i).gameObject);
        }
    }

    [Button]
    private void ResetPosition()
    {
        if (Waypoints.Count == 0) return;
        transform.position = Waypoints[0].transform.position;
    }

    private float g_angle = 0f;
    private Bounds g_platformBounds;
    void OnDrawGizmos()
    {
        if (Waypoints != null && Waypoints.Count > 1)
        {
            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i] != null)
                {
                    if (rotatingPlatform != null) g_angle = rotatingPlatform.g_angle;
                    else g_platformBounds = GetComponent<BoxCollider2D>().bounds;

                    // Draw a line between Waypoints
                    if (i < Waypoints.Count - 1 && Waypoints[i + 1] != null)
                    {
                        Debug.DrawLine(Waypoints[i].transform.position, Waypoints[i + 1].transform.position, previewColor);
                    }

                    // Draw an outline of the platform at each waypoint
                    Vector3 waypointPos = Waypoints[i].transform.position;
                    // Bounds platformBounds = GetComponent<BoxCollider2D>().bounds;

                    Vector3 topLeft = waypointPos + new Vector3(-g_platformBounds.extents.x, g_platformBounds.extents.y, 0);
                    Vector3 topRight = waypointPos + new Vector3(g_platformBounds.extents.x, g_platformBounds.extents.y, 0);
                    Vector3 bottomLeft = waypointPos + new Vector3(-g_platformBounds.extents.x, -g_platformBounds.extents.y, 0);
                    Vector3 bottomRight = waypointPos + new Vector3(g_platformBounds.extents.x, -g_platformBounds.extents.y, 0);

                    // Apply rotation to each corner
                    topLeft = RotatePointAroundPivot(topLeft, waypointPos, new Vector3(0, 0, g_angle));
                    topRight = RotatePointAroundPivot(topRight, waypointPos, new Vector3(0, 0, g_angle));
                    bottomLeft = RotatePointAroundPivot(bottomLeft, waypointPos, new Vector3(0, 0, g_angle));
                    bottomRight = RotatePointAroundPivot(bottomRight, waypointPos, new Vector3(0, 0, g_angle));

                    Debug.DrawLine(topLeft, topRight, previewColor);
                    Debug.DrawLine(topRight, bottomRight, previewColor);
                    Debug.DrawLine(bottomRight, bottomLeft, previewColor);
                    Debug.DrawLine(bottomLeft, topLeft, previewColor);

                }
            }

            // Draw a line from the last waypoint to the first one
            if (Waypoints[0] != null && Waypoints[^1] != null)
            {
                Debug.DrawLine(Waypoints[0].transform.position, Waypoints[^1].transform.position, previewColor);
            }

            Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
            {
                return Quaternion.Euler(angles) * (point - pivot) + pivot;
            }
        }

    }

#endif

    #endregion
}
