using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ResetLinePosition : MonoBehaviour
{
    public LineRenderer lineRenderer;

    [ContextMenu("ResetToObjectStart")]
    public void ResetToObjectStart()
    {
        if (lineRenderer == null || lineRenderer.positionCount == 0) return;
        int positionsNumber = lineRenderer.positionCount;
        Vector3 shiftVector = transform.position - lineRenderer.GetPosition(0);

        for (int i = 0; i < positionsNumber; i++)
        {
            Vector3 newPosition = lineRenderer.GetPosition(i) + shiftVector;
            lineRenderer.SetPosition(i, newPosition);
        }

    }

    [ContextMenu("ResetToObjectEnd")]
    public void ResetToObjectEnd()
    {
        if (lineRenderer == null || lineRenderer.positionCount == 0) return;
        int positionsNumber = lineRenderer.positionCount;

        Vector3 shiftVector = transform.position - lineRenderer.GetPosition(positionsNumber - 1);

        for (int i = 0; i < positionsNumber; i++)
        {
            Vector3 newPosition = lineRenderer.GetPosition(i) + shiftVector;
            lineRenderer.SetPosition(i, newPosition);
        }

    }

    [ContextMenu("ReversePositions")]
    void ReversePositions()
    {
        int positionCount = lineRenderer.positionCount;
        Vector3[] positions = new Vector3[positionCount];

        lineRenderer.GetPositions(positions);

        System.Array.Reverse(positions);

        lineRenderer.SetPositions(positions);
    }
}
