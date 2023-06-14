using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ResetLinePosition : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [ContextMenu("ResetToObjectStart")]
    public void ResetToObjectStart()
    {
        OnCall();
        if (_lineRenderer == null || _lineRenderer.positionCount == 0) return;
        int positionsNumber = _lineRenderer.positionCount;
        Vector3 shiftVector = transform.position - _lineRenderer.GetPosition(0);


        for (int i = 0; i < positionsNumber; i++)
        {
            Vector3 newPosition = _lineRenderer.GetPosition(i) + shiftVector;
            _lineRenderer.SetPosition(i, newPosition);
        }

    }

    [ContextMenu("ResetToObjectEnd")]
    public void ResetToObjectEnd()
    {
        OnCall();
        if (_lineRenderer == null || _lineRenderer.positionCount == 0) return;
        int positionsNumber = _lineRenderer.positionCount;

        Vector3 shiftVector = transform.position - _lineRenderer.GetPosition(positionsNumber - 1);

        for (int i = 0; i < positionsNumber; i++)
        {
            Vector3 newPosition = _lineRenderer.GetPosition(i) + shiftVector;
            _lineRenderer.SetPosition(i, newPosition);
        }

    }

    [ContextMenu("ReversePositions")]
    void ReversePositions()
    {
        OnCall();
        int positionCount = _lineRenderer.positionCount;
        Vector3[] positions = new Vector3[positionCount];

        _lineRenderer.GetPositions(positions);

        System.Array.Reverse(positions);

        _lineRenderer.SetPositions(positions);
    }

    [ContextMenu("LocalIntoWorldSpace")]
    void LocalIntoWorldSpace()
    {
        OnCall();
        if (_lineRenderer == null || _lineRenderer.positionCount == 0 || _lineRenderer.useWorldSpace) return;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, transform.TransformPoint(_lineRenderer.GetPosition(i)));
        }

        _lineRenderer.useWorldSpace = true;
    }


    [ContextMenu("WorldIntoLocalSpace")]
    void WorldIntoLocalSpace()
    {
        OnCall();
        if (_lineRenderer == null || _lineRenderer.positionCount == 0 || !_lineRenderer.useWorldSpace) return;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, transform.InverseTransformPoint(_lineRenderer.GetPosition(i)));
        }

        _lineRenderer.useWorldSpace = false;
    }

    void OnCall()
    {
        _lineRenderer = GetComponent<LineRenderer>();

    }
}
