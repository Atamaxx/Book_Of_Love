using UnityEngine;

public class PartialCircleCollider : MonoBehaviour
{
    public float collidableRegionAngle = 90f; // The angle of the collidable region in degrees

    private void Start()
    {
        CircleCollider2D fullCollider = GetComponent<CircleCollider2D>();

        // Create a child GameObject for the composite collider
        GameObject compositeColliderGO = new GameObject("CompositeCollider");
        compositeColliderGO.transform.SetParent(transform);
        compositeColliderGO.transform.localPosition = Vector3.zero;

        // Add a CompositeCollider2D to the composite collider GameObject
        CompositeCollider2D compositeCollider = compositeColliderGO.AddComponent<CompositeCollider2D>();

        // Calculate the number of points needed for the partial collidable region
        int pointCount = Mathf.CeilToInt(collidableRegionAngle / 10f); // Adjust the 10f value to control the smoothness

        // Create an array to hold the points of the partial collidable region
        Vector2[] points = new Vector2[pointCount + 1];

        // Calculate the angle increment between each point
        float angleIncrement = collidableRegionAngle / pointCount;

        // Calculate the angle of the first point in the collidable region
        float startAngle = -collidableRegionAngle / 2f;

        // Calculate the points of the partial collidable region
        for (int i = 0; i <= pointCount; i++)
        {
            float angle = startAngle + (i * angleIncrement);
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * fullCollider.radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * fullCollider.radius;
            points[i] = new Vector2(x, y);
        }

        // Create a child GameObject for the polygon collider
        GameObject polygonColliderGO = new GameObject("PolygonCollider");
        polygonColliderGO.transform.SetParent(compositeColliderGO.transform);
        polygonColliderGO.transform.localPosition = Vector3.zero;

        // Add a PolygonCollider2D to the polygon collider GameObject
        PolygonCollider2D polygonCollider = polygonColliderGO.AddComponent<PolygonCollider2D>();

        // Set the points of the polygon collider to the points of the partial collidable region
        polygonCollider.points = points;

        // Disable the child GameObject renderers
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }
    }
}
