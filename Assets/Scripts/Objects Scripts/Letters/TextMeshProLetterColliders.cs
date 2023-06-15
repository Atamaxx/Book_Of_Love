using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
[RequireComponent(typeof(MeshCollider))]
public class TextMeshProLetterColliders : MonoBehaviour
{
    private void Start()
    {
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Vector3[] vertices;
        int[] triangles;

        // Iterate through each character in the text
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            // Get the vertex and triangle data for the character
            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            vertices = textInfo.meshInfo[materialIndex].vertices;
            triangles = textInfo.meshInfo[materialIndex].triangles;

            // Create a polygon collider for the character
            GameObject letterCollider = new GameObject("Letter Collider");
            letterCollider.transform.SetParent(transform);
            letterCollider.layer = gameObject.layer;

            Mesh letterMesh = new Mesh();
            letterMesh.vertices = new Vector3[]
            {
                vertices[vertexIndex + 0],
                vertices[vertexIndex + 1],
                vertices[vertexIndex + 2],
                vertices[vertexIndex + 3]
            };

            letterMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            PolygonCollider2D polygonCollider = letterCollider.AddComponent<PolygonCollider2D>();
            polygonCollider.SetPath(0, ConvertToVector2Array(letterMesh.vertices));

            Destroy(letterMesh);
        }

        // Disable the original mesh collider
        meshCollider.enabled = false;
    }

    private Vector2[] ConvertToVector2Array(Vector3[] vertices)
    {
        Vector2[] vector2Vertices = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vector2Vertices[i] = vertices[i];
        }
        return vector2Vertices;
    }
}
