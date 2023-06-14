using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class AshesToAshes : MonoBehaviour
{
    [Header("SUN IS SHINNING")]
    public bool IsDawn = false;
    public bool IsDusk = false;
    [Header("SETTINGS")]
    public Transform ObjectToTeleport;
    public float TeleportationSpeed = 10f;
    public float SuckingRadius = 5f;
    public float LineWidth = 0.1f;
    public int LineSortingOrder = -6;
    public float EdgeRadius = 0.1f;

    public bool _canTeleport = true;
    private bool _coroutineEnded;
    private LineRenderer _lineRenderer;
    private EdgeCollider2D _edgeCollider;
    private Vector3[] _pointsPositions;
    private int _childCount;
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();

        Settings();

        if (IsDawn && IsDusk || !IsDawn && !IsDusk)
        {
            Destroy(gameObject);
        }
        else if (IsDawn)
        {
            Dawn();
        }
        else if (IsDusk)
        {
            Dusk();
        }

        ConnectChildren();
    }


    private void Update()
    {

        if (IsDawn) return;

        _coroutineEnded = Move.IsEnded();
        Teleporting();

    }

    private void Teleporting()
    {
    }

    private void Settings()
    {
        _lineRenderer.startWidth = LineWidth;
        _lineRenderer.endWidth = LineWidth;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.sortingOrder = LineSortingOrder;
        _edgeCollider.edgeRadius = EdgeRadius;
    }

    [ContextMenu("Update Lines")]
    private void ConnectChildren()
    {
        _childCount = transform.childCount;
        Vector3[] localPositions = new Vector3[_childCount];
        _pointsPositions = new Vector3[_childCount];

        for (int i = 0; i < _childCount; i++)
        {
            Transform child = transform.GetChild(i);
            _pointsPositions[i] = child.position;
            localPositions[i] = transform.InverseTransformPoint(child.position);
        }

        _lineRenderer.positionCount = _childCount;
        _lineRenderer.SetPositions(localPositions);


        Vector2[] colliderPoints = new Vector2[_lineRenderer.positionCount];
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            colliderPoints[i] = _lineRenderer.GetPosition(i);
        }

        // Set the points of the EdgeCollider2D
        _edgeCollider.points = colliderPoints;
    }

    private void Dawn()
    {
        _edgeCollider.isTrigger = false;
    }

    private void Dusk()
    {
        _edgeCollider.isTrigger = true;
    }

}
