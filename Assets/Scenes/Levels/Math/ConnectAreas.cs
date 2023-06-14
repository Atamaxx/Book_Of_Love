using System.Collections.Generic;
using UnityEngine;

public class ConnectAreas : MonoBehaviour
{
    public LineRenderer baseLine;
    public Renderer renderer2D;
    public LineRenderer heightLine;
    [SerializeField] private List<Vector2> _vertexPoints = new();
    private Material material;
    [SerializeField] private BookOf.Time _time;



    private float _mainArea;
    private float _cirleRadius;
    private int _verticesNum;
    private int _activeVertexNum;

    private void Start()
    {
        //_vertexPoints.Add(baseLine.GetPosition(0));
        //_vertexPoints.Add(baseLine.GetPosition(1));
        //_vertexPoints.Add(heightLine.GetPosition(1));
        _activeVertexNum = _time.ActivePointNum;


        SetVertices(_time.SetPoints);
        ShaderSetup();
    }

    private void Update()
    {
        SetVertices(_time.SetPoints);
        _verticesNum = _vertexPoints.Count;
        if (_verticesNum > 0)
            _vertexPoints[_activeVertexNum] = _time.TimePoint;
        _mainArea = MyMath.CalculatePolygonArea(_vertexPoints);
        _cirleRadius = MyMath.CircleRadiusByArea(_mainArea);

        ShaderUpdate();
    }

    private void SetVertices(List<Vector3> points)
    {
        _vertexPoints.Clear();
        for (int i = 0; i < points.Count; i++)
        {
            _vertexPoints.Add(points[i]);
        }
    }



    #region Shader
    private void ShaderSetup()
    {
        material = renderer2D.material;

        

    }
    private void ShaderUpdate()
    {

        if (_verticesNum < 3) return;// || MyMath.ArePointsOnSameLine(_vertexPoints)) return;
        
        material.SetFloat("_Diameter", 2 * _cirleRadius);
        material.SetVector("_PointA", _vertexPoints[0]);
        material.SetVector("_PointB", _vertexPoints[1]);
        material.SetVector("_PointC", _vertexPoints[2]);
    }
    #endregion
}
