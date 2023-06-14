using System.Collections.Generic;
using UnityEngine;


public class ColliderAdjuster : MonoBehaviour
{
    public List<Vector2> polyPoints = new();
    public List<Vector2> collisionPoints = new();
    public List<int> remove = new();




    public PolygonCollider2D polygon2D;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision involves the two colliders you're interested in
        if (collision.collider.CompareTag("Test 2"))
        {
            // Iterate through all contact points
            foreach (ContactPoint2D contactPoint in collision.contacts)
            {
                print("On Enter - " + contactPoint.point);
                ChangePoints(contactPoint.point);

                // Access contactPoint properties (e.g., contactPoint.point, contactPoint.normal)
                // Do something with the contact point information
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the collision involves the two colliders you're interested in
        if (collision.collider.CompareTag("Test 2"))
        {
            // Iterate through all contact points
            foreach (ContactPoint2D contactPoint in collision.contacts)
            {
                print("On Stay - " + contactPoint.point);

                //ChangePoints(contactPoint.point);
                // Access contactPoint properties (e.g., contactPoint.point, contactPoint.normal)
                // Do something with the contact point information
            }

        }
    }

    void ChangePoints(Vector2 point)
    {
        bool ifDelete = false;
        for (int i = 0; i < polyPoints.Count - 1; i++)
        {
            if (ifDelete)
            {
                remove.Add(i);
            }
            if (IsPointBetween(point, polyPoints[i], polyPoints[i + 1]))
            {
                //print("Point - " + point + " is between " + polyPoints[i] + " and " + polyPoints[i + 1]);
                ifDelete = !ifDelete;
                print("IIIIIIIIIIIIIIIIIII = " + i);

                collisionPoints.RemoveAt(i);
                collisionPoints.Insert(i, point);
            }
            
        }

        bool IsPointBetween(Vector2 point, Vector2 start, Vector2 end)
        {
            // Check if the point is between the start and end points
            bool isBetween = point.x >= Mathf.Min(start.x, end.x) && point.x <= Mathf.Max(start.x, end.x) &&
                             point.y >= Mathf.Min(start.y, end.y) && point.y <= Mathf.Max(start.y, end.y);

            return isBetween;
        }
    }


    private void Start()
    {
        // Get the PolygonCollider2D component attached to the GameObject
        polygon2D = GetComponent<PolygonCollider2D>();

        // Print all the points of the polygon collider
        PolygonPoints();
    }




    private void PolygonPoints()
    {
        if (polygon2D != null)
        {
            Vector2[] points = polygon2D.points;

            // Iterate through all the points
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 point = points[i];
                Vector2 worldPoint = polygon2D.transform.TransformPoint(point);

                // Print the point's position
                Debug.Log("Point " + i + ": " + point);
                // Print the world point's position
                Debug.Log("Point " + i + " (World Coordinates): " + worldPoint);
                polyPoints.Add(worldPoint);
                collisionPoints.Add(worldPoint);
            }
        }
    }







}
