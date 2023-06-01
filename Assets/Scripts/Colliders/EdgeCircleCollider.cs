
using UnityEngine;

public class EdgeCircleCollider : MonoBehaviour
{
    public float radius = 1f;
    public int pointCount = 50;

    public EdgeCollider2D edgeCollider;

    [ContextMenu("Create Circle Collider")]
    private void CreateCircleCollider()
    {
        // Number of points to create the circle collider

        Vector2[] points = new Vector2[pointCount + 1];

        for (int i = 0; i < pointCount; i++)
        {
            float angle = (Mathf.PI * 2 * i) / pointCount;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;

            points[i] = new Vector2(x, y);
        }
        points[pointCount] = points[0];
        edgeCollider.points = points;
    }
}

