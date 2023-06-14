using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingRigidbody : MonoBehaviour
{
    public LineRenderer LineRenderer;
    [Range(0, 2)] public float _percentClamp = 1f;
    [SerializeField] private BookOf.Time _timeLine;

    readonly List<Vector2> _line = new();
    readonly List<float> _lineDistances = new();
    private int _numberOfVertices;
    float _lineLength;
    Rigidbody2D rb2D;
    private float _percentPassed;


    private void Start()
    {
        SetUp();
        CalculateLineLength();
        _lineLength = _lineDistances[^1];

    }
    private void SetUp()
    {
        _numberOfVertices = LineRenderer.positionCount;

        for (int i = 0; i < _numberOfVertices; i++)
        {
            _line.Add(LineRenderer.GetPosition(i));
        }

        if (!LineRenderer.useWorldSpace)
        {
            for (int i = 0; i < _numberOfVertices; i++)
            {
                _line[i] = transform.TransformPoint(_line[i]);
            }
        }

        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _percentPassed = Mathf.Clamp(_timeLine.PercentPassed / _percentClamp, 0, 1);
        rb2D.MovePosition(PositionByPercent());
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

        if (!LineRenderer.useWorldSpace)        
            position = transform.InverseTransformPoint(position);
        
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

