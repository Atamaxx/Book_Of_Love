using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MovingTransform : MonoBehaviour
{
    [SerializeField] private BookOf.Time _timeLine;
    private LineRenderer _lineRenderer;
    [Header("SETTINGS")]
    [Range(0, 2)] public float PercentClamp = 1f;
    [Range(0, 200)] public float DistanceClamp = 1f;
    public bool MoveByDistance = true;
    public bool MoveByPercent = false;
    //public bool Loop = false;


    readonly List<Vector2> _line = new();
    readonly List<float> _lineDistances = new();
    private int _numberOfVertices;
    float _lineLength;

    private float _percentPassed;


    private void Start()
    {
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
        if (MoveByDistance)
        {
            _percentPassed = Mathf.Clamp(_timeLine.�urrentLength % DistanceClamp / DistanceClamp, 0, 1);
            transform.position = PositionByPercent();
        }
        else if (MoveByPercent)
        {
            _percentPassed = Mathf.Clamp(_timeLine.PercentPassed / PercentClamp, 0, 1);
            transform.position = PositionByPercent();
        }
        else return;

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

    Vector2 PositionByDistance()
    {
        _percentPassed = _timeLine.�urrentLength % DistanceClamp;

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
