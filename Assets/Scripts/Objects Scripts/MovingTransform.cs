using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class MovingTransform : MonoBehaviour
{
    [SerializeField] private BookOf.Time _timeLine;
    private LineRenderer _lineRenderer;
    [Header("MOVE BY DISTANCE")]
    public bool toMoveByDistance = true;
    public float MoveBC = 0f; // DistanceBottomClamp 
    public float MoveTC = 10f; // DistanceTopClamp
    [Header("ROTATION")]
    public bool toRotateByDistance = true;
    public float RotateBC = 0f; // RotateBottomClamp
    public float RotateTC = 10f; // RotateTopClamp
    public float RotateMin = 0f; // RotateTopClamp
    public float RotateMax = 180f; // RotateTopClamp
    [Header("MOVE BY PERCENT")]
    public bool toMoveByPercent = false;
    [Range(0, 2)] public float PercentClamp = 1f;
    //public bool Loop = false;
    [Header("100%")]
    public bool EndAction = false;
    public float DestroyDelay = 1.5f;


    readonly List<Vector2> _line = new();
    readonly List<float> _lineDistances = new();
    private int _numberOfVertices;
    float _lineLength;

    private float _percentPassed;
    private float _distancePassed;


    private void Start()
    {
        if (!toMoveByDistance && !toMoveByPercent) return;

        SetUp();
        CalculateLineLength();
        _lineLength = _lineDistances[^1];

    }
    private void SetUp()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _numberOfVertices = _lineRenderer.positionCount;

        for (int i = 0; i < _numberOfVertices; i++)
        {
            _line.Add(_lineRenderer.GetPosition(i));
        }

    }
    private void Update()
    {
        _distancePassed = _timeLine.ÑurrentLength;


        if (toMoveByDistance)
        {
            MoveByDistance();
        }
        else if (toMoveByPercent)
        {
            MoveByPercent();
        }
        //else return;

        if (toRotateByDistance)
        {
            RotateByDistance();
        }

    }

    void MoveByDistance()
    {
        if (MoveBC > _distancePassed)
        {
            transform.position = _line[0];
        }
        else if (_distancePassed > MoveTC)
        {
            transform.position = _line[^1];

            if (EndAction)
            {
                Destroy(gameObject, DestroyDelay);
            }
        }
        else if (MoveBC < _distancePassed && _distancePassed < MoveTC)
        {
            float clamp = MoveTC - MoveBC;
            _percentPassed = Mathf.Clamp((_distancePassed % MoveTC - MoveBC) / clamp, 0, 1);
            transform.position = PositionByPercent();
        }
    }
    void MoveByPercent()
    {
        _percentPassed = Mathf.Clamp(_timeLine.PercentPassed / PercentClamp, 0.01f, 1);
        transform.position = PositionByPercent();
    }

    void RotateByDistance()
    {
        float rotationValue;
        float distanceValue = Mathf.Clamp(_distancePassed, RotateBC, RotateTC);

        rotationValue = Mathf.Lerp(RotateMin, RotateMax, distanceValue / (RotateTC - RotateBC));

        Vector3 targetRotation = new (0f, 0f, rotationValue);
        transform.rotation = Quaternion.Euler(targetRotation);
    }
    Vector2 PositionByPercent()
    {

        Vector2 position = Vector2.zero;

        float percentLength = _percentPassed * _lineLength;

        for (int i = 1; i < _numberOfVertices; i++)
        {
            float currDist = _lineDistances[i];

            if (percentLength <= currDist)
            {
                float prevDist = _lineDistances[i - 1];
                float fraction = (percentLength - prevDist) / (currDist - prevDist);

                float startX = _line[i - 1].x;
                float startY = _line[i - 1].y;
                float endX = _line[i].x;
                float endY = _line[i].y;

                position = new Vector2(startX + fraction * (endX - startX), startY + fraction * (endY - startY));
                break;
            }
        }



        return position;
    }


    private void CalculateLineLength()
    {
        float length = 0f;
        _lineDistances.Add(length);
        for (int i = 0; i < _numberOfVertices - 1; i++)
        {
            Vector3 segmentStart = _line[i];
            Vector3 segmentEnd = _line[i + 1];
            length += Vector3.Distance(segmentStart, segmentEnd);
            _lineDistances.Add(length);
        }
    }
}

