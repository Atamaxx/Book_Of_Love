using System.Collections.Generic;
using UnityEngine;

public class Tem : MonoBehaviour
{
    public Transform pivotPoint; // The point around which the rotation will occur
    public float angle; // The angle by which you want to rotate (in degrees)

    public List<Transform> pointsToRotate = new();

    private void Update()
    {
        for (int i = 0; i < pointsToRotate.Count; i++)
        {
            RotatePoint(pointsToRotate[i]);
        }
    }

    private void RotatePoint(Transform pointToRotate)
    {
        // Calculate the direction vector from the pivot point to the point to rotate
        Vector2 direction = pointToRotate.position - pivotPoint.position;

        // Convert the angle from degrees to radians
        float radians = angle * Mathf.Deg2Rad;

        // Apply the rotation to the direction vector
        float cosTheta = Mathf.Cos(radians);
        float sinTheta = Mathf.Sin(radians);
        float x = direction.x * cosTheta - direction.y * sinTheta;
        float y = direction.x * sinTheta + direction.y * cosTheta;

        // Get the new position of the point by adding the rotated direction vector to the pivot point
        Vector2 newPosition = pivotPoint.position + new Vector3(x, y);

        // Set the new position of the point
        pointToRotate.position = newPosition;
    }
}
