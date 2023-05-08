using System.Collections.Generic;
using UnityEngine;

public class NewLine : MonoBehaviour
{
    public Transform playerPos;
    public float playerX;
    public float prevPlayerX;
    public Transform allPoints;
    public List<Vector3> points = new();
    public LineRenderer lineRenderer;

    void Start()
    {
        Draw();
        TimePoint = points[0];
        LineLength = CalculateLineLength();
        DefineDirections();
    }
    private void Update()
    {
        prevPlayerX = playerX;
        playerX = playerPos.position.x;
        FinalCalculation();
        LastLinePassed = LinePassed;
        LinePassed = LineProgress();
        if (LinePassed - LastLinePassed > 0)
        {
            Step = 1;
        }
        else if (LinePassed - LastLinePassed < 0)
        {
            Step = -1;
        }
    }

    #region Draw

    public void Draw()
    {
        GetAllChildTransforms(allPoints);
        DrawLine();
    }
    public void DrawLine()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        int positionsNum = points.Count;
        lineRenderer.positionCount = positionsNum;
        for (int i = 0; i < positionsNum; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }


    void GetAllChildTransforms(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Do something with the child's Transform here
            points.Add(child.position);
        }
    }
    #endregion

    List<int> FindNearestPointIndices()
    {
        List<int> nearestPointIndices = new();
        nearestPointIndices.Clear();

        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 point1 = lineRenderer.GetPosition(i);
            Vector3 point2 = lineRenderer.GetPosition(i + 1);
            float xMin = Mathf.Min(point1.x, point2.x);
            float xMax = Mathf.Max(point1.x, point2.x);

            if (playerX >= xMin && playerX <= xMax)
            {
                nearestPointIndices.Add(i);
            }
        }

        return nearestPointIndices;
    }
    Vector2 CalculateTimePoint(Vector2 point1, Vector2 point2)
    {
        Vector2 timePoint;
        float slope = (point2.y - point1.y) / (point2.x - point1.x);
        float yIntercept = point1.y - slope * point1.x;
        Debug.Log("y = " + slope + "x + " + yIntercept);
        timePoint = new Vector2(playerX, slope * playerX + yIntercept);
        return timePoint;
    }

    public Vector3 TimePoint;
    public int LPoint;
    public int RPoint;
    public int LetUsHandleThis;
    public int _currentSegment;
    void FinalCalculation()
    {

        List<int> pointIndices = FindNearestPointIndices();






        ChooseTimeSegment();


        if (pointIndices.Count == 1 && LetUsHandleThis == 0)
        {
            print("00000000");
            _currentSegment = pointIndices[0];
            TimePoint = CalculateTimePoint(points[_currentSegment], points[_currentSegment + 1]);
        }
        else if (pointIndices.Count > 1 || LetUsHandleThis > 0)
        {
            TimePoint = CalculateTimePoint(points[_currentSegment], points[_currentSegment + 1]);

            if (LetUsHandleThis == 2)
            {
                TimePoint = points[_currentSegment + 1];
            }
            if (LetUsHandleThis == 3)
            {
                TimePoint = points[_currentSegment];
            }
        }
        else
        {
            TimePoint = Vector3.zero;
        }


    }

    public int markedPointsIndex = 0;
    private void ChooseTimeSegment()
    {
        if (markedPointsIndex < markedPoints.Count)
        {
            print("SDAS");
            Checking();
        }


        //if (markedPointsIndex > 0)
        //{
        //    Checking(markedPointsIndex - 1,0);
        //}


        void Checking()
        {
            int currentMarkedPoint;
            int index;
            int currCase;
            if (markedPoints[markedPointsIndex] == _currentSegment + 1)
            {
                currentMarkedPoint = markedPoints[markedPointsIndex];
                index = markedPointsIndex;
                currCase = 0;
            }
            else if (markedPoints[markedPointsIndex - 1] == _currentSegment)
            {
                currentMarkedPoint = markedPoints[markedPointsIndex - 1];
                index = markedPointsIndex-1;
                currCase = 1;
            }
            else
                return;

            print("currentSegment - " + _currentSegment);
            print("currentMarkedPoint " + points[currentMarkedPoint]);

            if (PointXPassed(points[currentMarkedPoint].x))
            {
                markedPointsPasses[index]++;
                int currentMarkedPointPasses = markedPointsPasses[index];

                if (currentMarkedPointPasses % 2 == 0 && currentMarkedPointPasses % 4 != 0)
                {
                    print("+++1");
                    LetUsHandleThis = 0;
                    _currentSegment++;
                    markedPointsIndex++;
                }
                else if (currentMarkedPointPasses % 2 == 1)
                {
                    print("0001");
                    LetUsHandleThis = currCase == 0 ? 2: 3;
                    return;
                }
                else if (currentMarkedPointPasses % 4 == 0)
                {
                    print("---1");
                    LetUsHandleThis = 0;
                    markedPointsIndex--;

                    _currentSegment--;
                }


            }
        }

        bool PointXPassed(float passPointX)
        {
            if ((passPointX > prevPlayerX && passPointX < playerX)
                || (passPointX < prevPlayerX && passPointX > playerX))
            {
                LetUsHandleThis = 1;
                print("PASSED");
                return true;
            }
            print("NOT PASSED");

            return false;
        }
    }

    List<int> directions = new(); // -1 left, 0 no direction, 1 right
    List<float> segmentsLength = new();
    List<int> markedPoints = new();
    public List<int> markedPointsPasses = new();
    public GameObject Mark;
    private void DefineDirections()
    {
        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 segmentStart = lineRenderer.GetPosition(i);
            Vector3 segmentEnd = lineRenderer.GetPosition(i + 1);

            segmentsLength.Add(Vector3.Distance(segmentStart, segmentEnd));

            if (segmentEnd.x - segmentStart.x > 0)
                directions.Add(1);
            else if (segmentEnd.x - segmentStart.x < 0)
                directions.Add(-1);
            else
                directions.Add(0);
        }
        // Mark changing points
        for (int i = 0; i < directions.Count - 1; i++)
        {
            if (directions[i] + directions[i + 1] == 0)
            {
                markedPoints.Add(i + 1);
                markedPointsPasses.Add(0);
            }
        }

        foreach (int mark in markedPoints)
        {
            Instantiate(Mark, points[mark], Quaternion.identity);
        }


    }



    #region Line Parameters
    [Header("Line Progress")]
    public float LinePassed;
    public float LastLinePassed;
    public float LineLength;
    public int Step;

    private float LineProgress()
    {
        float length = 0f;
        for (int i = 0; i < LPoint; i++)
        {
            Vector3 segmentStart = lineRenderer.GetPosition(i);
            Vector3 segmentEnd = lineRenderer.GetPosition(i + 1);

            length += Vector3.Distance(segmentStart, segmentEnd);
        }
        length += Vector3.Distance(points[LPoint], TimePoint);
        return length;
    }

    List<float> _segmentsLength = new();
    private float CalculateLineLength()
    {
        float length = 0f;

        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 segmentStart = lineRenderer.GetPosition(i);
            Vector3 segmentEnd = lineRenderer.GetPosition(i + 1);
            _segmentsLength.Add(length);
            length += Vector3.Distance(segmentStart, segmentEnd);
        }
        _segmentsLength.Add(length);

        return length;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(playerPos.position, TimePoint);
    }
}
