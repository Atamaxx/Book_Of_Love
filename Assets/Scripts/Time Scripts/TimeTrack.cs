using System.Collections.Generic;
using UnityEngine;

public class TimeTrack : MonoBehaviour
{
    private void Awake()
    {
        _timeLine = GetComponent<LineRenderer>();
    }
    void Start()
    {
        TimeTrackParameters();
        _closestPoint = _trackPoints[0];
    }

    private void Update()
    {
        UpdatePositions();
        Stopwatch();
        RunningUpThatHill();
        CalculateTrackPercentage();
    }

    #region Parameters

    [Header("Main Parameters")]
    [SerializeField] private Transform player; // Maybe its better to use groundCheck to avoid TIME jumps (Fix-1)
    public float TIME;

    private Vector2 playerPos;

    private void UpdatePositions()
    {
        playerPos = player.position;
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


    #region Time Line

    [Header("Time Line")]
    [SerializeField] private float _trackPercentage;
    [SerializeField] private float _trackLength;
    [SerializeField] private float _trackPassed;
    [SerializeField] private float _lastTrackPercentage;
    [SerializeField] private float _timeGap;
    [SerializeField] private float _timeGapThreshold = 0.01f;
    //[SerializeField] private int _iterationSteps = 100;

    [SerializeField] private Vector2 _closestPoint;
    [SerializeField] private Vector2 _lastClosestPoint;
    private Vector2 _lastPositions;
    readonly List<Vector2> _trackPoints = new();

    LineRenderer _timeLine;


    private void RunningUpThatHill()
    {
        // Find the closest point on the track to the player
        Vector2 projectedPoint;
        float closestDist = Vector2.Distance(playerPos, _closestPoint);
        for (int i = 0; i < _trackPoints.Count - 1; i++)
        {
            Vector2 point1 = _trackPoints[i];
            Vector2 point2 = _trackPoints[i + 1];

            // Project player position onto line defined by point1 and point2
            Vector2 line = point2 - point1;
            float projection = Vector2.Dot(playerPos - point1, line) / line.sqrMagnitude;

            // Clamp projection to line segment
            projection = Mathf.Clamp01(projection);
            projectedPoint = point1 + projection * line;

            // Check if projectedPoint is closer than closestPoint
            float dist = Vector2.Distance(playerPos, projectedPoint);
            if (dist < closestDist) //&& projection != 1)
            {
                //_lastClosestPoint = _closestPoint;
                closestDist = dist;
                _closestPoint = projectedPoint;
            }
        }
    }

    private void CalculateTrackPercentage()
    {
        // Calculate percentage of track passed
        float trackGap;
        _lastTrackPercentage = _trackPercentage;

        _trackPercentage = 0;

        // If we reached extreme points
        if (_closestPoint == _trackPoints[0])
        {
            _trackPercentage = 0;
            return;
        }
        else if (_closestPoint == _trackPoints[^1])
        {
            _trackPercentage = 1;
            return;
        }

        for (int i = 0; i < _trackPoints.Count - 1; i++)
        {
            Vector2 point1 = _trackPoints[i];
            Vector2 point2 = _trackPoints[i + 1];

            if (IsPointBetweenTwoOthers(point1, _closestPoint, point2))
            {
                _trackPercentage += Vector2.Distance(point1, _closestPoint) / _trackLength;

                // Fill time gap
                trackGap = _trackPercentage - _lastTrackPercentage;
                _trackPassed = _trackPercentage * _trackLength;

                if (trackGap > _timeGapThreshold)
                    _timeGap = trackGap * _trackLength;
                return;
            }
            else
            {
                _trackPercentage += Vector2.Distance(point1, point2) / _trackLength;
                _trackPassed = _trackPercentage * _trackLength;
            }
        }
    }

 



    private void TimeTrackParameters()
    {
        for (int i = 0; i < _timeLine.positionCount; i++)
        {
            _trackPoints.Add(_timeLine.GetPosition(i));
        }

        // Total track length
        _trackLength = 0;
        for (int i = 0; i < _trackPoints.Count - 1; i++)
        {
            _trackLength += Vector2.Distance(_trackPoints[i], _trackPoints[i + 1]);
        }
    }

    private bool IsPointBetweenTwoOthers(Vector2 a, Vector2 between, Vector2 c)
    {
        between -= a;
        c -= a;

        float cRatio = Mathf.Round(c.x / c.y * 100f) / 100f;
        float betweenRatio = Mathf.Round(between.x / between.y * 100f) / 100f;

        if (cRatio == betweenRatio)
            return true;
        else
            return false;
    }

    #endregion


    private void OnDrawGizmos()
    {
        Debug.DrawLine(playerPos, _closestPoint, Color.red);
    }
}

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

//    playerPosX = player.position.x;
//    playerPos = player.position;


//    currentDistance = playerPosX - startX;
//    progress = currentDistance / levelDistance;

//    speedDistance = playerPosX - lastPlayerPosX;
//    playerSpeed = speedDistance / Time.deltaTime;
//    lastPlayerPosX = playerPosX;

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


//    Debug.DrawLine(playerPos, point1, Color.black, 1f);
//    Debug.DrawLine(playerPos, point2, Color.gray, 1f);

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

//            Debug.DrawLine(playerPos, currentPoint, Color.green, 1f);
//            yield return null;
//        }
//        _trackPercentage = _lastTrackPercentage;
//    }

//}