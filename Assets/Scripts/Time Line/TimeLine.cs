using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _Oh;
    [SerializeField] private GameObject _Tutututu;


    private Vector2 _timePoint;
    [SerializeField] private int _currentSegment = 0;
    [SerializeField] private int _betweenPoint = 0;
    [SerializeField] private int _numberOfSegments;
    [SerializeField] private int _numberOfVertices;
    private bool _canContinue;
    private float _playerX;

    private readonly List<int> _markedPoints = new();
    private readonly List<bool> _ifDirectionRight = new();
    private readonly float _edgesToleranceValue = 0.05f;
    private readonly List<Vector3> _line = new();

    [Header("Line Progress")]
    public float LineLength;
    public static float ÑurrentLength;
    public static float PercentPassed;

    public List<bool> IfDirectionRight => _ifDirectionRight;


    #region Start
    private void Start()
    {
        SetUp();
        DetectEdgePoints();
        FinalCalculations();
        SetDown();
    }

    private void SetUp()
    {
        _numberOfVertices = _lineRenderer.positionCount;

        for (int i = 0; i < _numberOfVertices; i++)
        {
            _line.Add(_lineRenderer.GetPosition(i));
        }
        _markedPoints.Add(0);

        if (_line[1].x >= _line[0].x)
            IfDirectionRight.Add(true);
        else
            IfDirectionRight.Add(false);

        LineLength = CalculateLineLength();

    }
    private void SetDown()
    {
        _markedPoints.Add(_numberOfVertices - 1);

        if (_line[_numberOfVertices - 1].x < _line[_numberOfVertices - 2].x)
            IfDirectionRight.Add(true);
        else
            IfDirectionRight.Add(false);
    }
    private void DetectEdgePoints()
    {
        float xOfCurrentVertex;
        float xOfNextVertex;
        bool goRight = true;
        bool prevGoRight;
        for (int i = 0; i < _numberOfVertices - 1; i++)
        {
            xOfCurrentVertex = _line[i].x;
            xOfNextVertex = _line[i + 1].x;
            prevGoRight = goRight;

            if (xOfNextVertex - xOfCurrentVertex > _edgesToleranceValue)
                goRight = true;
            else if (xOfNextVertex - xOfCurrentVertex < -_edgesToleranceValue)
                goRight = false;
            else
                goRight = prevGoRight;

            if (goRight != prevGoRight)
                MarkPoint(i, goRight);
        }
    }


    private void MarkPoint(int pointNum, bool isRight)
    {
        Instantiate(_Oh, _line[pointNum], Quaternion.identity);
        _markedPoints.Add(pointNum);
        IfDirectionRight.Add(isRight);
    }

    private void FinalCalculations()
    {
        _numberOfSegments = _markedPoints.Count;
    }
    #endregion


    #region Update
    private void Update()
    {
        _playerX = _playerTransform.position.x;
        FindTimePoint();
        ChangeSegment();
        ÑurrentLength = LineProgress();
        PercentPassed = ÑurrentLength / LineLength;
        _Tutututu.transform.position = _timePoint;
    }

    private void FindTimePoint()
    {
        for (int i = _markedPoints[_currentSegment]; i < _markedPoints[_currentSegment + 1]; i++)
        {
            Vector2 firstPoint = _line[i];
            Vector2 secondPoint = _line[i + 1];

            if ((firstPoint.x <= _playerX && _playerX <= secondPoint.x)
               || (secondPoint.x <= _playerX && _playerX <= firstPoint.x))
            {
                _betweenPoint = i;
                _timePoint = CalculateTimePoint(firstPoint, secondPoint);
                _canContinue = true;
            }
        }
    }


    private void ChangeSegment()
    {
        if (!_canContinue) return;

        if (_currentSegment > 0)
        {
            int pointNum0 = _markedPoints[_currentSegment];
            bool pointDirRight0 = IfDirectionRight[_currentSegment];

            if (pointDirRight0 && _playerX < _line[pointNum0].x)
            {
                _timePoint = _line[pointNum0];
                _currentSegment--;
                _canContinue = false;
            }
            else if (!pointDirRight0 && _playerX > _line[pointNum0].x)
            {
                _timePoint = _line[pointNum0];
                _currentSegment--;
                _canContinue = false;
            }
        }

        if (!_canContinue) return;

        if (_currentSegment < _numberOfSegments - 1)
        {
            int pointNum1 = _markedPoints[_currentSegment + 1];
            bool pointDirRight1 = IfDirectionRight[_currentSegment + 1];

            if (pointDirRight1 && _playerX < _line[pointNum1].x)
            {
                _timePoint = _line[pointNum1];
                _currentSegment++;
                _canContinue = false;
            }
            else if (!pointDirRight1 && _playerX > _line[pointNum1].x)
            {
                _timePoint = _line[pointNum1];
                _currentSegment++;
                _canContinue = false;
            }
        }
    }

    Vector2 CalculateTimePoint(Vector2 point1, Vector2 point2)
    {
        Vector2 timePoint;
        float slope = (point2.y - point1.y) / (point2.x - point1.x);
        float yIntercept = point1.y - slope * point1.x;
        timePoint = new Vector2(_playerX, slope * _playerX + yIntercept);
        return timePoint;
    }



    private float LineProgress()
    {
        float length = 0f;
        for (int i = 0; i < _betweenPoint; i++)
        {
            Vector3 segmentStart = _line[i];
            Vector3 segmentEnd = _line[i + 1];

            length += Vector3.Distance(segmentStart, segmentEnd);
        }

        if (_betweenPoint > 1)
        {
            length += Vector3.Distance(_line[_betweenPoint], _timePoint);
        }
        else
        {
            length += Vector3.Distance(_line[_betweenPoint], _timePoint);
        }
        return length;
    }

    private float CalculateLineLength()
    {
        float length = 0f;

        for (int i = 0; i < _numberOfVertices - 1; i++)
        {
            Vector3 segmentStart = _line[i];
            Vector3 segmentEnd = _line[i + 1];
            length += Vector3.Distance(segmentStart, segmentEnd);
        }

        return length;
    }

    #endregion



    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_playerTransform.position, _timePoint);
    }
}
