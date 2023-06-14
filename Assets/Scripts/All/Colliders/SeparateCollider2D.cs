using System.Collections.Generic;
using UnityEngine;

public class SeparateCollider2D : MonoBehaviour
{
    public Transform RayOrigin;
    public Transform origin;
    public Transform LineStart;
    public Transform LineEnd;
    public float RayLength = 1f;
    public LayerMask LettersLayer;
    public List<Vector2> intersectionPoints = new List<Vector2>();




    private PolygonCollider2D polyCollider;


    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        SeparatePaths();
    }

    void SeparatePaths()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(RayOrigin.position, Vector2.right, RayLength, LettersLayer);

        foreach (RaycastHit2D hit in hits)
        {
            Vector2 hitPoint = hit.point;
            
            intersectionPoints.Add(hitPoint);
        }
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(RayOrigin.position, Vector2.right, Color.blue);
        foreach (Vector2 point in intersectionPoints)
        {
            Debug.DrawLine(origin.position, point, Color.red);
        }
    }
}

