using System.Collections.Generic;
using UnityEngine;



public class TimeTrack : MonoBehaviour
{
    private void Awake()
    {
        _timeLine = GetComponentInChildren<LineRenderer>();
    }

    void Start()
    {
        _numberOfPoints = _timeLine.positionCount;

        TimeTrackParameters();
        // CombinePointsWithSameX(ref _trackPoints);
        currPlayerX = player.position.x;

        foreach (var item in _trackPoints)
        {
            Debug.DrawLine(item, item + Vector2.up * 2, Color.white, 100f);

        }
        _numberOfPoints = _trackPoints.Count;
    }

    private void Update()
    {
        UpdatePositions();
        Stopwatch();
        UpdateSegment(_trackPoints, player, ref prevPlayerX, ref currPlayerX);
        CalculateTrackPercentage(_trackPoints, _timePoint);
    }


    #region Parameters

    [Header("Main Parameters")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask _groundLayer;
    public float TIME;

    [SerializeField] private Vector2 _playerPos;

    private void UpdatePositions()
    {
        _playerPos = player.position;
    }
    private void TimeTrackParameters()
    {
        for (int i = 0; i < _numberOfPoints; i++)
        {
            _trackPoints.Add(_timeLine.GetPosition(i));
        }

        // Total track length
        _trackLength = 0;
        for (int i = 0; i < _numberOfPoints - 1; i++)
        {
            _trackLength += Vector2.Distance(_trackPoints[i], _trackPoints[i + 1]);
        }
        _activatedPoint1 = _trackPoints[0];
        _activatedPoint2 = _trackPoints[1];
        _timePoint = _trackPoints[0];
    }

    void CombinePointsWithSameX(ref List<Vector2> points)
    {
        List<Vector2> result = new();

        for (int i = 0; i < points.Count; i++)
        {
            if (i == 0 || i == points.Count - 1 || points[i].x != points[i - 1].x || points[i].x != points[i + 1].x)
            {
                result.Add(points[i]);
            }
        }

        points = result;
    }

    #endregion

    #region Stopwatch

    [Header("Stopwatch")]

    public float stopTime = 0f;
    public float runningTime;
    public float wholeTime;
    public bool isStopwatchRunning = true;

    private void Stopwatch()
    {
        wholeTime = Time.time;

        if (isStopwatchRunning)
        {
            runningTime = wholeTime - stopTime;
        }

        if (!isStopwatchRunning)
        {
            stopTime = wholeTime - runningTime;
        }
    }

    #endregion


    float prevPlayerX = 0f;
    float currPlayerX;
    [SerializeField] private int segmentPoint1 = 0;
    [SerializeField] private int segmentPoint2 = 1;
    [SerializeField] int cornerPass = 0;
    [SerializeField] int sameXPass = 0;
    [SerializeField] float _tolerenceDistance = 0.1f;
    [SerializeField] bool startTimeLine = false;



    void UpdateSegment(List<Vector2> trackPoints, Transform playerTransform, ref float prevPlayerX, ref float currPlayerX)
    {
        // Get the player's x position in the previous and current frames

        prevPlayerX = currPlayerX;
        currPlayerX = playerTransform.position.x;
        List<int> passedPoints = new();



        // Iterate through list of track points
        for (int i = 0; i < trackPoints.Count; i++)//(int i = trackPoints.Count - 1; i >= 0; i--)
        {
            // Check if the player crossed the x position of this track point
            if (((prevPlayerX < trackPoints[i].x && currPlayerX >= trackPoints[i].x)
                || (prevPlayerX > trackPoints[i].x && currPlayerX <= trackPoints[i].x)))
            {
                // First check
                if (i == 0 && !startTimeLine)
                {
                    startTimeLine = true;
                    goto breakIf;
                }

                passedPoints.Add(i);
                Debug.Log("Player passed track point #" + i);
            }

            if (!startTimeLine)
            {
                _timePoint = trackPoints[0];
                return;
            }
        }
        int passedNum = passedPoints.Count;
        int pointIndex = -1;
        // If only one point passed
        if (passedNum == 1)
        {
            print("only one point passed = " + passedPoints[0]);

            IfCornerPoint(passedPoints[0]);
            if (cornerPass == 2)
            {
                cornerPass = 0;
                goto breakIf;
            }
            if (passedPoints[0] == segmentPoint1 && segmentPoint1 > 0)
            {
                print("+");
                SetSegment(passedPoints[0] - 1, passedPoints[0]);
            }
            else if (passedPoints[0] == segmentPoint2)
            {
                print("-");
                SetSegment(passedPoints[passedNum - 1], passedPoints[passedNum - 1] + 1);
            }
        }

        // If more than one point passed
        if (passedNum > 1)
        {
            if (sameXPass == 1)
            {
                sameXPass = 0;
                goto breakIf;
            }
            if (FindPointIndex(segmentPoint2))
            {
                print("++");
                int pointValue = LastIncrementalValue(pointIndex, 1);
                IfSameXForward(segmentPoint2, pointValue);
                SetSegment(pointValue, pointValue + 1);
            }
            else if (FindPointIndex(segmentPoint1))
            {
                print("--");
                int pointValue = LastDecrementalValue(pointIndex, 1);
                IfSameXBackward(segmentPoint1, pointValue);

                SetSegment(pointValue - 1, pointValue);
            }
        }
    breakIf:

        if (segmentPoint1 >= trackPoints.Count - 1)
        {
            _timePoint = trackPoints[^1];
            return;
        }

        for (int i = 0; i < passedPoints.Count; i++)
        {
            print("passedPoints = " + passedPoints[i]);
        }

        _timePoint = FindIntersection(trackPoints[segmentPoint1], trackPoints[segmentPoint2], currPlayerX);

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Help
        bool FindPointIndex(int point)
        {
            int index = passedPoints.IndexOf(point);
            if (index >= 0)
            {
                pointIndex = index;
                return true;
            }
            return false;
        }

        int LastIncrementalValue(int start, int increment)
        {
            for (int i = start; i < passedPoints.Count - 1; i++)
            {
                if (passedPoints[i + 1] - passedPoints[i] == increment)
                {
                    pointIndex++;
                }
                else break;
            }

            // Last incremental value
            return passedPoints[pointIndex];
        }
        int LastDecrementalValue(int start, int decrement)
        {

            for (int i = start; i > 0; i--)
            {
                if (passedPoints[i] - passedPoints[i - 1] == decrement)
                {
                    pointIndex--;
                }
                else break;
            }
            // Last decremental value
            return passedPoints[pointIndex];
        }
        void SetSegment(int seg1, int seg2)
        {
            if (seg1 < 0 || seg2 < 1)
                return;
            segmentPoint1 = seg1;
            segmentPoint2 = seg2;
        }

        void IfSameXForward(int lastSeg, int newSeg)
        {
            if (lastSeg < 1 || newSeg >= trackPoints.Count - 1) return;


            Vector2 lastVector = trackPoints[lastSeg - 1] - trackPoints[lastSeg];
            Vector2 newVector = trackPoints[newSeg + 1] - trackPoints[newSeg];
            // Vectors are pointing right
            if (lastVector.x > 0 && newVector.x > 0)
            {
                _timePoint = trackPoints[newSeg];
                sameXPass++;
            }
            // Vectors are pointing left
            else if (lastVector.x < 0 && newVector.x < 0)
            {
                _timePoint = trackPoints[newSeg];
                sameXPass++;
            }

        }

        void IfSameXBackward(int lastSeg, int newSeg)
        {
            if (newSeg < 1 || lastSeg >= trackPoints.Count - 1) return;

            Vector2 lastVector = trackPoints[lastSeg + 1] - trackPoints[lastSeg];
            Vector2 newVector = trackPoints[newSeg - 1] - trackPoints[newSeg];
            //Vectors are pointing right
            if (lastVector.x > 0 && newVector.x > 0)
            {
                _timePoint = trackPoints[newSeg];
                sameXPass++;
            }
            // Vectors are pointing left
            else if (lastVector.x < 0 && newVector.x < 0)
            {
                _timePoint = trackPoints[newSeg];
                sameXPass++;
            }
        }

        void IfCornerPoint(int pointNum)
        {
            if (pointNum <= 1 || pointNum >= _numberOfPoints - 1)
                return;
            float pointPosX = trackPoints[pointNum].x;
            if (pointPosX < trackPoints[pointNum + 1].x && pointPosX < trackPoints[pointNum - 1].x)
            {
                cornerPass++;
            }
            else if (pointPosX > trackPoints[pointNum + 1].x && pointPosX > trackPoints[pointNum - 1].x)
            {
                cornerPass++;
            }
        }
        #endregion
    }





    #region Time Line

    [Header("Time Line")]
    [SerializeField] private float _trackLength;
    [SerializeField] private float _trackPercentage;
    [SerializeField] private float _trackPassed;
    [SerializeField] private float _toleranceValue;

    List<Vector2> _trackPoints = new();
    int _numberOfPoints;
    LineRenderer _timeLine;

    [SerializeField] private Vector2 _timePoint;
    [SerializeField] private Vector2 _activatedPoint1;
    [SerializeField] private Vector2 _activatedPoint2;
    private bool _activateTimeTrack;




    #endregion

    private void CalculateTrackPercentage(List<Vector2> trackPoints, Vector2 currentPoint)
    {
        // Calculate percentage of track passed

        _trackPercentage = 0;

        // If we reached extreme points
        if (Vector2Approximately(currentPoint, trackPoints[0]))
        {
            _trackPassed = 0;
            _trackPercentage = 0;
            return;
        }
        else if (Vector2Approximately(currentPoint, trackPoints[^1]))
        {
            _trackPassed = _trackLength;
            _trackPercentage = 1;
            return;
        }

        float distancePassed = 0;
        for (int i = 0; i < segmentPoint2; i++)
        {
            Vector2 point1 = trackPoints[i];
            Vector2 point2 = trackPoints[i + 1];

            if (IsPointBetween(point1, currentPoint, point2))
            {
                Debug.Log("SASD");
                distancePassed += Vector2.Distance(point1, currentPoint);
                break;
            }
            else
            {
                Debug.Log("::::::");

                distancePassed += Vector2.Distance(point1, point2);
            }
        }

        _trackPassed = distancePassed;
        _trackPercentage = _trackPassed / _trackLength;
    }

    #region Additional


    private bool IsPointBetweenTwoOthers(Vector2 a, Vector2 between, Vector2 c)
    {
        between -= a;
        c -= a;

        //float cRatio = Mathf.Round(c.x / c.y * 100f) / 100f;
        //float betweenRatio = Mathf.Round(between.x / between.y * 100f) / 100f;
        float cRatio = c.x / c.y;
        float betweenRatio = between.x / between.y;

        if (Mathf.Approximately(cRatio, betweenRatio))
            return true;
        else
            return false;
    }
    bool IsPointBetween(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 A = p2 - p1;
        Vector2 B = p3 - p1;
        float dotProduct = Vector2.Dot(A, B);
        float magA = A.magnitude;
        float magB = B.magnitude;
        float cosAngle = dotProduct / (magA * magB);
        return cosAngle >= 0 && cosAngle <= 1;
    }
    private bool Vector2Approximately(Vector2 a, Vector2 b)
    {
        if (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y))
        {
            return true;
        }

        return false;
    }

    private Vector2 FindIntersection(Vector2 p1, Vector2 p2, float x)
    {
        // check if line segment is vertical
        if (p1.x == p2.x)
        {
            return _timePoint; // no intersection
        }

        // calculate slope and y-intercept of line segment
        float m = (p2.y - p1.y) / (p2.x - p1.x);
        float b = p1.y - m * p1.x;

        // calculate x-coordinate of intersection point
        float ix = x;

        // calculate y-coordinate of intersection point
        float iy = m * ix + b;

        // check if intersection point lies within line segment bounds
        if (ix < Mathf.Min(p1.x, p2.x) || ix > Mathf.Max(p1.x, p2.x) ||
            iy < Mathf.Min(p1.y, p2.y) || iy > Mathf.Max(p1.y, p2.y))
        {
            return _timePoint; // no intersection
        }
        // return intersection point
        return new Vector2(ix, iy);
    }
    #endregion


    private void OnDrawGizmos()
    {
        // Debug.DrawLine(_playerPos, _closestPoint, Color.red);
        Debug.DrawLine(_playerPos, _timePoint, Color.red);
    }
}



//private void FirstCheck()
//{
//    if (Mathf.Abs(_playerPos.x - _activatedPoint1.x) < _toleranceValue)
//    {
//        _activateTimeTrack = true;
//    }
//}
//private void RunningUpThatHill()
//{
//    // Find the closest point on the track to the player
//    Vector2 projectedPoint;
//    float closestDist = Vector2.Distance(_playerPos, _closestPoint);
//    for (int i = 0; i < _trackPoints.Count - 1; i++)
//    {
//        Vector2 point1 = _trackPoints[i];
//        Vector2 point2 = _trackPoints[i + 1];

//        // Project player position onto line defined by point1 and point2
//        Vector2 line = point2 - point1;
//        float projection = Vector2.Dot(_playerPos - point1, line) / line.sqrMagnitude;

//        // Clamp projection to line segment
//        projection = Mathf.Clamp01(projection);
//        projectedPoint = point1 + projection * line;

//        // Check if projectedPoint is closer than closestPoint
//        float dist = Vector2.Distance(_playerPos, projectedPoint);
//        if (dist < closestDist) //&& projection != 1)
//        {
//            //_lastClosestPoint = _closestPoint;
//            closestDist = dist;
//            _closestPoint = projectedPoint;
//        }
//    }
//}

//private float TimeEquation(float distance)
//{
//    Vector2 start;
//    Vector2 end;
//    float slope;

//    float time;

//    //start = startMarker.position;
//    start = player.position;
//    end = endMarker.position;

//    slope = (end.y - start.y) / (end.x - start.x);

//    time = slope * (distance - start.x) + start.y;

//    return time;
//}
//private void TrackProgress()
//{
//    float speedDistance;

//    _playerPosX = player.position.x;
//    _playerPos = player.position;


//    currentDistance = _playerPosX - startX;
//    progress = currentDistance / levelDistance;

//    speedDistance = _playerPosX - lastPlayerPosX;
//    playerSpeed = speedDistance / Time.deltaTime;
//    lastPlayerPosX = _playerPosX;

//    avarageSpeed = currentDistance / runningTime;

//    levelTime = levelDistance / playerSpeed;
//    TIME = currentDistance / avarageSpeed;
//}

//private void ChangeMusicSpeed()
//{
//    float musicSpeed;
//    float trackDuration = musicController.SongGuration();

//    //musicSpeed = trackDuration / levelTime;
//    musicSpeed = Mathf.RoundToInt(playerSpeed / 12);

//    if (musicSpeed == 0)
//    {
//        musicController.ChangeSpeed(0.75f);
//        return;
//    }

//    musicController.ChangeSpeed(musicSpeed);
//}
//private void TimeGap(Vector2 point1, Vector2 point2, Vector2 positions1, Vector2 positions2)
//{
//    int pos1X = (int)positions1.x;
//    int pos1Y = (int)positions1.y;
//    int pos2X = (int)positions2.x;
//    int pos2Y = (int)positions2.y;
//    bool goRight = true;

//    if (positions1.x < positions2.y)
//        goRight = true;
//    else if (positions1.y > positions2.x)
//        goRight = false;


//}

//private IEnumerator FillTimeGap(Vector2 point1, Vector2 point2, Vector2 positions1, Vector2 positions2, float trackGap)
//{
//    if (trackGap < _timeGapThreshold) yield break;


//    Debug.DrawLine(_playerPos, point1, Color.black, 1f);
//    Debug.DrawLine(_playerPos, point2, Color.gray, 1f);

//    int pos1X = (int)positions1.x;
//    int pos1Y = (int)positions1.y;
//    int pos2X = (int)positions2.x;
//    int pos2Y = (int)positions2.y;

//    bool goRight = true;

//    if (positions1.x < positions2.y)
//        goRight = true;
//    else if (positions1.y > positions2.x)
//        goRight = false;

//    if (positions1 == positions2)
//    {
//        StartCoroutine(IterateBetweenPoints(point1, point2));
//        yield break;
//    }

//    if (goRight)
//    {
//        Debug.Log("SS");
//        yield return StartCoroutine(IterateBetweenPoints(point1, _trackPoints[pos1Y]));

//        if (Mathf.Abs(pos1X - pos2X) > 1)
//        {
//            for (int i = pos1Y; i < pos2X; i++)
//            {
//                yield return StartCoroutine(IterateBetweenPoints(_trackPoints[i], _trackPoints[i + 1]));
//            }
//        }
//        Debug.Log("AA");

//        yield return StartCoroutine(IterateBetweenPoints(_trackPoints[pos2X], point2));
//    }
//    else
//    {
//        yield return StartCoroutine(IterateBetweenPoints(point1, _trackPoints[pos1X]));
//        if (Mathf.Abs(pos1X - pos2X) > 1)
//        {
//            for (int i = pos1X; i > pos2Y; i--)
//            {
//                yield return StartCoroutine(IterateBetweenPoints(_trackPoints[i], _trackPoints[i - 1]));

//            }
//        }
//        yield return StartCoroutine(IterateBetweenPoints(_trackPoints[pos2Y], point2));
//    }


//    IEnumerator IterateBetweenPoints(Vector2 startPoint, Vector2 endPoint)
//    {
//        int iterationsNum = Mathf.RoundToInt(trackGap * 100);
//        Vector2 currentPoint;
//        Vector2 diff = (endPoint - startPoint) / iterationsNum;

//        for (int i = 0; i < iterationsNum; i++)
//        {
//            currentPoint = startPoint + (diff * i);
//            if (goRight)
//                _trackPercentage += Vector2.Distance(startPoint, currentPoint) / _trackLength;
//            else _trackPercentage -= Vector2.Distance(startPoint, currentPoint) / _trackLength;

//            Debug.DrawLine(_playerPos, currentPoint, Color.green, 1f);
//            yield return null;
//        }
//        _trackPercentage = _lastTrackPercentage;
//    }

//}