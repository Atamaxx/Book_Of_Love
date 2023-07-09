using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [Header("MAIN PROPERTIES")]
    [SerializeField] private bool activateX = true;
    [SerializeField] private bool activateY = false;

    [Header("SERIALIZE")]
    [SerializeField] private LineRenderer _lineRenderer;
    public GameObject Tutututu;              // Object to make time point visiable
    public Transform playerTransform;

    [Header("RESULTS")]
    public float LineLength;
    public Vector2 TimePoint;                            // Current point on the line
    public float ÑurrentLength;
    public float PercentPassed;

    /////////////////////////////////

    [SerializeField] private int _currentSegment = 0;           // Represents first point of current segment where directions is not changing
    [SerializeField] private int _betweenPoint = 0;             // Represents left point of two that player currently in the middle of
    private int _numberOfSegments;
    private int _numberOfVertices;
    private bool _canContinue;

    private readonly List<int> _markedPoints = new();
    private readonly List<bool> _ifDirectionRight = new();
    private readonly float _edgesToleranceValue = 0.05f;
    private readonly List<Vector3> _line = new();
    private readonly List<Vector3> _baseLine = new();
    private readonly List<float> _lineCoord = new();

    private Vector2 _playerPosition;

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
        playerTransform = GameObject.FindGameObjectWithTag(Info.PlayerTag).transform;
        _playerPosition = playerTransform.position;
        Tutututu = Instantiate(Tutututu, TimePoint, Quaternion.identity);

        _numberOfVertices = _lineRenderer.positionCount;

        for (int i = 0; i < _numberOfVertices; i++)
        {
            _line.Add(_lineRenderer.GetPosition(i));
            _baseLine.Add(_line[i]);
        }
        _markedPoints.Add(0);

        if (activateY)
        {
            for (int i = 0; i < _numberOfVertices; i++)
            {
                _line[i] = RotatePoint(_line[0], _line[i], 90f);
            }
        }

        for (int i = 0; i < _numberOfVertices; i++)
        {
            _lineCoord.Add(_line[i].x);
        }

        if (_lineCoord[1] >= _lineCoord[0])
            _ifDirectionRight.Add(true);
        else
            _ifDirectionRight.Add(false);

        LineLength = CalculateLineLength();


    }

    private void SetDown()
    {
        _markedPoints.Add(_numberOfVertices - 1);

        if (_lineCoord[_numberOfVertices - 1] < _lineCoord[_numberOfVertices - 2])
            _ifDirectionRight.Add(true);
        else
            _ifDirectionRight.Add(false);
    }
    private void DetectEdgePoints()
    {
        float coordOfCurrentVertex;
        float coordOfNextVertex;
        bool goRight = true;
        bool prevGoRight;

        if (_lineCoord[1] - _lineCoord[0] > _edgesToleranceValue)
            goRight = true;
        else if (_lineCoord[1] - _lineCoord[0] < -_edgesToleranceValue)
            goRight = false;


        for (int i = 1; i < _numberOfVertices - 1; i++)
        {
            coordOfCurrentVertex = _lineCoord[i];
            coordOfNextVertex = _lineCoord[i + 1];
            prevGoRight = goRight;

            float coordSubtraction = coordOfNextVertex - coordOfCurrentVertex;

            if (coordSubtraction > _edgesToleranceValue)
                goRight = true;
            else if (coordSubtraction < -_edgesToleranceValue)
                goRight = false;
            else
                goRight = prevGoRight;

            if (goRight != prevGoRight)
                MarkPoint(i, goRight);
        }
    }


    private void MarkPoint(int pointNum, bool isRight)
    {
        _markedPoints.Add(pointNum);
        _ifDirectionRight.Add(isRight);
    }

    private void FinalCalculations()
    {
        _numberOfSegments = _markedPoints.Count;
    }
    #endregion


    #region Update

    private void Update()
    {
        if (!activateX && !activateY)
            return;
        //_playerPosition = Info.PlayerPosition;
        _playerPosition = playerTransform.position;

        float playerPosForY = RotatePoint(_line[0], _playerPosition, angle).x;

        if (activateX)
            _playerCoord = _playerPosition.x;
        else if (activateY)
            _playerCoord = playerPosForY;

        if (!OutOfBounds())
        {
            FindTimePoint();
            ChangeSegment();
            ÑurrentLength = LineProgress();
            PercentPassed = ÑurrentLength / LineLength;
        }
        Tutututu.transform.position = TimePoint;
    }

    private void FindTimePoint()
    {
        for (int i = _markedPoints[_currentSegment]; i < _markedPoints[_currentSegment + 1]; i++)
        {
            float firstPoint = _lineCoord[i];
            float secondPoint = _lineCoord[i + 1];

            if ((firstPoint <= _playerCoord && _playerCoord <= secondPoint)
               || (secondPoint <= _playerCoord && _playerCoord <= firstPoint))
            {
                _betweenPoint = i;
                TimePoint = CalculateTimePoint(_line[i], _line[i + 1]);

                RotateTimePointIfY();


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
            bool pointDirRight0 = _ifDirectionRight[_currentSegment];

            if (pointDirRight0 && _playerCoord < _lineCoord[pointNum0])
            {
                TimePoint = _line[pointNum0];
                _betweenPoint = pointNum0;
                _currentSegment--;
                _canContinue = false;

                RotateTimePointIfY();
            }
            else if (!pointDirRight0 && _playerCoord > _lineCoord[pointNum0])
            {
                TimePoint = _line[pointNum0];
                _betweenPoint = pointNum0;
                _currentSegment--;
                _canContinue = false;

                RotateTimePointIfY();
            }

        }



        if (!_canContinue) return;

        if (_currentSegment < _numberOfSegments - 1)
        {
            int pointNum1 = _markedPoints[_currentSegment + 1];
            bool pointDirRight1 = _ifDirectionRight[_currentSegment + 1];

            if (pointDirRight1 && _playerCoord < _lineCoord[pointNum1])
            {
                TimePoint = _line[pointNum1];
                _betweenPoint = pointNum1;
                _currentSegment++;
                _canContinue = false;

                RotateTimePointIfY();
            }
            else if (!pointDirRight1 && _playerCoord > _lineCoord[pointNum1])
            {
                TimePoint = _line[pointNum1];
                _betweenPoint = pointNum1;
                _currentSegment++;
                _canContinue = false;

                RotateTimePointIfY();
            }
        }
    }

    Vector2 CalculateTimePoint(Vector2 point1, Vector2 point2)
    {
        Vector2 timePoint;
        float slope = (point2.y - point1.y) / (point2.x - point1.x);
        float yIntercept = point1.y - slope * point1.x;
        timePoint = new Vector2(_playerCoord, slope * _playerCoord + yIntercept);
        return timePoint;
    }



    private float LineProgress()
    {
        float length = 0f;
        for (int i = 0; i < _betweenPoint; i++)
        {
            Vector3 segmentStart = _baseLine[i];
            Vector3 segmentEnd = _baseLine[i + 1];

            length += Vector3.Distance(segmentStart, segmentEnd);
        }

        if (_betweenPoint > 1)
        {
            length += Vector3.Distance(_baseLine[_betweenPoint], TimePoint);
        }
        else
        {
            length += Vector3.Distance(_baseLine[_betweenPoint], TimePoint);
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

    private bool OutOfBounds()
    {
        if (_currentSegment == 0 &&
            (_ifDirectionRight[0] && _playerCoord < _lineCoord[0] || !_ifDirectionRight[0] && _playerCoord > _lineCoord[0]))
        {
            _betweenPoint = 0;
            ÑurrentLength = 0;
            TimePoint = _line[0];
            PercentPassed = 0;
        }
        else if (_currentSegment + 1 == _numberOfSegments &&
            (_ifDirectionRight[^1] && _playerCoord < _lineCoord[^1] || !_ifDirectionRight[^1] && _playerCoord > _lineCoord[^1]))
        {
            ÑurrentLength = LineLength;
            TimePoint = _line[^1];
            if (activateY)
                TimePoint = RotatePoint(_line[0], TimePoint, -angle);

            PercentPassed = 1;
        }
        else return false;

        return true;
    }

    #endregion


    #region Coordinates Changing
    private float _playerCoord;




    public float angle = 90f; // The angle to rotate by (in degrees)

    private void RotateTimePointIfY()
    {
        if (activateY)
        {
            TimePoint = RotatePoint(_line[0], TimePoint, -angle);
        }
    }

    private Vector2 RotatePoint(Vector3 pivotPoint, Vector3 pointToRotate, float angle)
    {
        // Calculate the direction vector from the pivot point to the point to rotate
        Vector2 direction = pointToRotate - pivotPoint;

        // Convert the angle from degrees to radians
        float radians = angle * Mathf.Deg2Rad;

        // Apply the rotation to the direction vector
        float cosTheta = Mathf.Cos(radians);
        float sinTheta = Mathf.Sin(radians);
        float x = direction.x * cosTheta - direction.y * sinTheta;
        float y = direction.x * sinTheta + direction.y * cosTheta;

        // Get the new position of the point by adding the rotated direction vector to the pivot point
        Vector2 newPosition = pivotPoint + new Vector3(x, y);

        return newPosition;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_playerPosition, TimePoint);
    }
}
