using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class LRDisplayValues : MonoBehaviour
{
    public TextMeshPro numberTextPrefab;

    private LineRenderer _line;
    public float IntervalDistance = 10f;
    public float MarkersDivider = 1f;
    private float length;
    private TextMeshPro currentText;



    private void OnContext()
    {
        _line = GetComponent<LineRenderer>();
        length = My.Line.CalculateLength(_line);
    }

    [Button]
    private void MarkByDistance()
    {
        OnContext();
        int numOfPoints = (int)(length / IntervalDistance);

        GameObject parentObject = new("Distance Markers");

        for (int i = 0; i < numOfPoints + 1; i++)
        {
            float dist = i * IntervalDistance;
            currentText = Instantiate(numberTextPrefab, My.Line.FindPointByLength(_line, dist), Quaternion.identity);
            currentText.transform.SetParent(parentObject.transform);
            string text = (dist / MarkersDivider).ToString();
            currentText.name = text;
            currentText.text = text;
        }
    }
    [Button]
    private void DeleteMarkers()
    {
        GameObject parentObject = GameObject.Find("Distance Markers");
        if (parentObject == null) return;

        DestroyImmediate(parentObject);
    }

    [Button]
    private void DuplicateLastPoint()
    {
        OnContext();
        int lastPos = _line.positionCount;
        _line.positionCount++;
        _line.SetPosition(lastPos, _line.GetPosition(lastPos - 1));
    }

    [Button("Delete Last Point")]
    private void DelLastPoint()
    {
        OnContext();
        if (_line.positionCount == 0) return;
        _line.positionCount--;
    }
    [Button("Set Z to 0")]
    private void Zto0()
    {
        OnContext();
        for (int i = 0; i < _line.positionCount; i++)
        {
            Vector3 point = _line.GetPosition(i);
            point.z = 0;
            _line.SetPosition(i, point);
        }
    }

}
