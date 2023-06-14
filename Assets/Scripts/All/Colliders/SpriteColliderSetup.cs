using UnityEngine;

public class SpriteColliderSetup : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EdgeCollider2D edgeCollider;
    public GameObject SOMETHING;
    private Vector2 _objectPos; 
    void Start()
    {
        SetupEdgeCollider();
    }

  
    void SetupEdgeCollider()
    {

        // Get the sprite's vertices in local space
        Vector2[] spriteVertices = spriteRenderer.sprite.vertices;

        // Convert the sprite's vertices to shape points in local space
        Vector2[] shapePoints = new Vector2[spriteVertices.Length];

        for (int i = 0; i < spriteVertices.Length; i++)
        {
            //shapePoints[i] = spriteRenderer.transform.TransformPoint(spriteVertices[i]);
            //shapePoints[i] = shapePoints[i];
            Instantiate(SOMETHING, new Vector3(spriteVertices[i].x, spriteVertices[i].y, 0), Quaternion.identity);
        }
       // shapePoints = _objectPos + shapePoints.;
        edgeCollider.points = spriteVertices;
    }
}
