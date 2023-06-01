using UnityEngine;

public class ConnectAreas : MonoBehaviour
{
    //public CircleCollider2D circleCollider;
    public LineRenderer baseLine;
    public Renderer renderer2D;
    public LineRenderer heightLine;
    public Vector2 currentHeigtPoint;
    public Vector2 CircleCentre;
    public Vector2 TriangleBase;
    private Vector2[] sidePoints = new Vector2[3];
    [SerializeField] private float _cirleArea;
    private Material material;
    [SerializeField] private Time _timeLine;

    private void Start()
    {
        material = renderer2D.material;
        sidePoints[0] = baseLine.GetPosition(0);
        sidePoints[1] = baseLine.GetPosition(1);
        sidePoints[2] = heightLine.GetPosition(1);

        baseLength = Vector2.Distance(sidePoints[0], sidePoints[1]);
        heightLength = Vector2.Distance(sidePoints[0], sidePoints[2]);

        ShaderSetup();
    }

    private void Update()
    {
        currentHeigtPoint = _timeLine.TimePoint;

        ShaderUpdate();
    }

    #region Calculations
    private float CircleRadiusByArea(float desiredArea)
    {
        float radius = Mathf.Sqrt(desiredArea / Mathf.PI);
        return radius;
    }


    float baseLength;
    float heightLength;
    private float IsoscelesTrapezoidArea()
    {
        float area;
        float smallBaseLength = Vector2.Distance(FindPointByX(sidePoints[0], sidePoints[2], currentHeigtPoint.x), currentHeigtPoint) * 2;
        float height = Vector2.Distance((sidePoints[0] + sidePoints[1]) / 2, currentHeigtPoint);
        area = height * (baseLength + smallBaseLength) / 2;
        return area;
    }

    Vector2 FindPointByX(Vector2 point1, Vector2 point2, float xValue)
    {
        Vector2 point;
        float slope = (point2.y - point1.y) / (point2.x - point1.x);
        float yIntercept = point1.y - slope * point1.x;
        point = new Vector2(xValue, slope * xValue + yIntercept);
        return point;
    }
    #endregion


    #region Shader
    private void ShaderSetup()
    {
        material.SetFloat("_Height", heightLength);
        material.SetFloat("_Width", baseLength);
        material.SetVector("_CircleCentre", CircleCentre);
        material.SetVector("_TriangleBase", TriangleBase);
    }
    private void ShaderUpdate()
    {
        material.SetFloat("_Diameter", 2 * CircleRadiusByArea(IsoscelesTrapezoidArea()));

    }
    #endregion
}
