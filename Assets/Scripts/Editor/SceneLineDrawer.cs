using UnityEditor;
using UnityEngine;

public class SceneLineDrawer : MonoBehaviour
{
    private Vector2 start;
    private Vector2 end;
    private bool isDrawing;

    void OnDrawGizmos()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            start = Event.current.mousePosition;
            isDrawing = true;
        }
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            end = Event.current.mousePosition;
            isDrawing = false;
            DrawLine();
        }

        if (isDrawing)
        {
            Handles.DrawLine(start, Event.current.mousePosition);
        }
    }

    void DrawLine()
    {
        GameObject lineObject = new GameObject("Line");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Camera.current.ScreenToWorldPoint(new Vector3(start.x, start.y, 0)));
        lineRenderer.SetPosition(1, Camera.current.ScreenToWorldPoint(new Vector3(end.x, end.y, 0)));
    }
}
