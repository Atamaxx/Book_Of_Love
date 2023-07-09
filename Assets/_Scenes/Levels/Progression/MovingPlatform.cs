#if UNITY_EDITOR
#endif
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private BookOf.Time _timeLine;
    public List<GameObject> Waypoints = new();
    public GameObject WaypointParent;
    public Sprite WaypointSprite;
    public Color previewColor;


    ///
    [BoxGroup("CONSTANT MOVEMENT")] public bool toConstantMove;
    [BoxGroup("CONSTANT MOVEMENT"), ShowIf("toConstantMove")]
    public float Speed = 25f;
    ///
    [BoxGroup("MOVE BY DISTANCE"), OnValueChanged("OnMove")] public bool toMoveByDistance;

    void OnMove() { toLoop = false; }

    [BoxGroup("MOVE BY DISTANCE"), ShowIf("toMoveByDistance")]
    public float MoveBC = 0f; // DistanceBottomClamp 
    [BoxGroup("MOVE BY DISTANCE"), ShowIf("toMoveByDistance")]
    public float MoveTC = 10f; // DistanceTopClamp

    ///
    [BoxGroup("LOOP"), ShowIf("toMoveByDistance")] public bool toLoop; //OnValueChanged("OnLoop")
                                                                       //[BoxGroup("LOOP"), ShowIf("toLoop")]
                                                                       // public float LoopBC = 0f; 
    [BoxGroup("LOOP"), ShowIf("toLoop")]
    public float RepeatEach = 10f;
    //   [BoxGroup("LOOP"), ShowIf("toLoop")]
    //public float LoopTC = 50f;
    //
    [BoxGroup("ROTATION")]
    public bool toRotateByDistance;
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public bool isClockwise = false;
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public float RotateBC = 0f; // RotateBottomClamp
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public float RotateTC = 10f; // RotateTopClamp
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public float RotateMin = 0f; // RotateTopClamp
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public float RotateMax = 180f; // RotateTopClamp

    //
    [BoxGroup("100%")]
    public bool EndAction = false;
    [BoxGroup("100%"), ShowIf("EndAction")]
    public float DestroyDelay = 1.5f;


    private int waypointIndex = 0;
    private int _numberOfPoints = 0;
    private readonly List<Vector3> _waypoints = new();
    private float _timeDistance;
    private float _platformDist;
    private float _platformPercent;
    private float _wayLength;

    private void Start()
    {
        //if (!toMoveByDistance && !toMoveByPercent) return;
        g_platformBounds = GetComponent<BoxCollider2D>().bounds;
        _numberOfPoints = Waypoints.Count;

        for (int i = 0; i < _numberOfPoints; i++)
        {
            _waypoints.Add(Waypoints[i].transform.position);
        }

        if (toLoop)
        {
            _waypoints.Add(Waypoints[0].transform.position);
        }

        _wayLength = My.Line.CalculateLength(_waypoints);
    }

    //private void OnLoop()
    //{
    //    if (toLoop)
    //    {
    //        _waypoints.Add(Waypoints[0].transform.position);
    //    }
    //    else
    //    {
    //        _waypoints.RemoveAt(_waypoints.Count - 1);
    //    }
    //}


    void Update()
    {
        _timeDistance = _timeLine.ÑurrentLength;


        if (toConstantMove)
            My.Transformations.MoveConstant(Waypoints, ref waypointIndex, transform, Speed);
        else if (toMoveByDistance)
        {
            if (toLoop)
            {
                MoveDistanceLoop();

            }
            else
                MoveDistance();
        }

        if (toRotateByDistance)
        {
            RotateByDistance();
        }
    }

    void MoveDistance()
    {
        if (_timeDistance <= MoveBC)
            transform.position = _waypoints[0];
        else if (_timeDistance >= MoveTC)
            transform.position = _waypoints[^1];
        else if (MoveBC < _timeDistance && _timeDistance < MoveTC)
        {
            _platformPercent = Mathf.Clamp((_timeDistance % MoveTC - MoveBC) / (MoveTC - MoveBC), 0, 1);

            _platformDist = _platformPercent * _wayLength;
            transform.position = PositionByPercent();
        }
    }

    void MoveDistanceLoop()
    {
        if (_timeDistance <= MoveBC)
        {
            transform.position = _waypoints[0];
            return;
        }
        else if (MoveTC <= _timeDistance)
        {
            _platformPercent = Mathf.Clamp((MoveTC - MoveBC) % RepeatEach / RepeatEach, 0, 1);
            _platformDist = _platformPercent * _wayLength;
        }
        else if (MoveBC < _timeDistance && _timeDistance < MoveTC)
        {
            _platformPercent = Mathf.Clamp((_timeDistance - MoveBC) % RepeatEach / RepeatEach, 0, 1);
            _platformDist = _platformPercent * _wayLength;
        }
        transform.position = PositionByPercent();
    }

    void RotateByDistance()
    {
        float rotationValue;
        float distanceValue = Mathf.Clamp(_timeDistance, RotateBC, RotateTC);
        rotationValue = Mathf.Lerp(RotateMin, RotateMax, (distanceValue - RotateBC) / (RotateTC - RotateBC));
        Vector3 targetRotation;

        if (isClockwise)
            rotationValue = -rotationValue;

        g_angle = rotationValue;

        targetRotation = new(0f, 0f, rotationValue);

        transform.rotation = Quaternion.Euler(targetRotation);
    }

    Vector2 PositionByPercent()
    {
        return My.Line.FindPointByLength(_waypoints, _platformDist);
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
                    g_platformBounds = GetComponent<BoxCollider2D>().bounds;

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
