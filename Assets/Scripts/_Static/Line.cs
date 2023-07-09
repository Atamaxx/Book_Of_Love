using System.Collections.Generic;
using UnityEngine;

namespace My
{
    public class Line : MonoBehaviour
    {
        public static float CalculateLength(LineRenderer lineRenderer)
        {
            float length = 0f;
            for (int i = 1; i < lineRenderer.positionCount; i++)
            {
                length += Vector3.Distance(lineRenderer.GetPosition(i - 1), lineRenderer.GetPosition(i));
            }
            return length;
        }
        public static float CalculateLength(List<Vector3> points)
        {
            float length = 0f;
            for (int i = 1; i < points.Count; i++)
            {
                length += Vector3.Distance(points[i - 1], points[i]);
            }
            return length;
        }
        public static Vector3 FindPointByLength(LineRenderer lineRenderer, float length)
        {
            float currLength = 0f;
            float subLength;
            int numberOfPoints = lineRenderer.positionCount;
            for (int i = 1; i < numberOfPoints; i++)
            {
                Vector2 point0 = lineRenderer.GetPosition(i - 1);
                Vector2 point1 = lineRenderer.GetPosition(i);
                subLength = currLength;
                currLength += Vector3.Distance(point0, point1);
                if (currLength > length)
                {
                    return PointByDistance(point0, length - subLength, point1);
                }
            }

            return lineRenderer.GetPosition(numberOfPoints - 1);
        }

        public static Vector3 FindPointByLength(List<Vector3> points, float length)
        {
            float currLength = 0f;
            float subLength;
            int numberOfPoints = points.Count;
            for (int i = 1; i < numberOfPoints; i++)
            {
                Vector2 point0 = points[i - 1];
                Vector2 point1 = points[i];
                subLength = currLength;
                currLength += Vector3.Distance(point0, point1);
                if (currLength > length)
                {
                    return PointByDistance(point0, length - subLength, point1);
                }
            }

            return points[numberOfPoints - 1];
        }

        public static Vector2 PointByDistance(Vector2 startPoint, float distance, Vector2 endPoint)
        {
            float fullDistance = Vector2.Distance(startPoint, endPoint);

            float x0 = startPoint.x;
            float y0 = startPoint.y;
            float x1 = endPoint.x;
            float y1 = endPoint.y;
            float t = distance / fullDistance;

            return new Vector2(x0 - t * x0 + t * x1, (y0 - t * y0 + t * y1));        //(((1?t)x0 + tx1),((1?t)y0 + ty1))
        }

    }
}