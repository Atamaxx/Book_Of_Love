using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class DynamicEdgeCollider : MonoBehaviour
{
    public BoxCollider2D[] collidersToConnect;

    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        UpdateEdgeCollider();
    }

    private void UpdateEdgeCollider()
    {
        // Create a new list of points
        var points = new List<Vector2>();

        // Loop through each Box Collider 2D
        foreach (var boxCollider in collidersToConnect)
        {
            // Get the four corners of the Box Collider 2D
            var boxPoints = new Vector2[4]
            {
                boxCollider.offset + new Vector2(-boxCollider.size.x, -boxCollider.size.y) * 0.5f,
                boxCollider.offset + new Vector2(boxCollider.size.x, -boxCollider.size.y) * 0.5f,
                boxCollider.offset + new Vector2(boxCollider.size.x, boxCollider.size.y) * 0.5f,
                boxCollider.offset + new Vector2(-boxCollider.size.x, boxCollider.size.y) * 0.5f
            };

            // Add the points to the list
            points.AddRange(boxPoints);
        }

        // Update the points of the Edge Collider 2D
        edgeCollider.points = points.ToArray();
    }
}
