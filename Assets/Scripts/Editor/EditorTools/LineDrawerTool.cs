using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Line Drawer", typeof(LineRenderer))]
public class LineDrawerTool : EditorTool
{
    private GameObject selectedGameObject;
    private LineRenderer lineRenderer;
    private int currentIndex = 0;
    public Texture2D ToolIcon;


    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = ToolIcon,
                text = "Line Drawer Tool",
                tooltip = "Click on Game Object with LineRenderer to draw a line"
            };
        }
    }
    public void OnEnable()
    {
        selectedGameObject = Selection.activeGameObject;
        selectedGameObject.TryGetComponent(out lineRenderer);
        currentIndex = lineRenderer.positionCount;
    }

    public override void OnToolGUI(EditorWindow window)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && !currentEvent.alt)
        {
            Vector3 mouseHit = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;
            ContinueLine(mouseHit);
            Event.current.Use();

        }

        if (currentEvent.type == EventType.MouseDrag && currentEvent.button == 0 && !currentEvent.alt)
        {
            Vector3 mouseHit = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition).origin;

            ContinueLine(mouseHit);
            Event.current.Use();
        }

        if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0 && !currentEvent.alt)
        {
            currentIndex = lineRenderer.positionCount;
            Event.current.Use();
        }

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.LeftShift && !currentEvent.alt)
        {
            CreateNewLine();
            Event.current.Use();
        }
    }

    void CreateNewLine()
    {
        currentIndex = 0;
        lineRenderer.positionCount = 0;
    }

    void ContinueLine(Vector3 lineVertexPos)
    {
        lineVertexPos.z = 0;
        currentIndex = lineRenderer.positionCount;
        lineRenderer.positionCount = currentIndex + 1;
        lineRenderer.SetPosition(currentIndex, lineVertexPos);

        currentIndex++;
    }

    void StartLine(Vector3 lineVertexPos)
    {
        lineVertexPos.z = 0;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(currentIndex, lineVertexPos);

        currentIndex++;
    }
}