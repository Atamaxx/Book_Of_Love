using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class LineCollider : MonoBehaviour
{
    private EdgeCollider2D edgeCollider;
    private LineRenderer myLine;

    [ContextMenu("Setup Collider")]
    void SetEdgeCollider()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        myLine = GetComponent<LineRenderer>();
        List<Vector2> edges = new();

        for (int point = 0; point < myLine.positionCount; point++)
        {
            Vector3 lineRendererPoint = myLine.GetPosition(point);
            if (myLine.useWorldSpace)
            {
                lineRendererPoint = transform.InverseTransformPoint(lineRendererPoint);
            }
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);
    }
}