using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LRDisplayValues : MonoBehaviour
{
    public TextMeshPro numberTextPrefab;

    private LineRenderer _line;
    public float intervalDistance = 10f;
    private float length;
    private TextMeshPro currentText;



    private void OnContext()
    {
        _line = GetComponent<LineRenderer>();
        length = My.Line.CalculateLength(_line);
    }

    [ContextMenu("Mark By Distance")]
    private void MarkByDistance()
    {
        OnContext();
        int numOfPoints = (int)(length / intervalDistance);

        GameObject parentObject = new ("Distance Markers");

        for (int i = 0; i < numOfPoints + 1; i++)
        {
            float dist = i * intervalDistance;
            currentText = Instantiate(numberTextPrefab, My.Line.FindPointByLength(_line, dist), Quaternion.identity);
            currentText.transform.SetParent(parentObject.transform);
            currentText.text = dist.ToString();
        }
    }

}
