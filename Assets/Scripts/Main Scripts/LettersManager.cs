using UnityEngine;

public class LettersManager : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;
    public Sprite newSprite;
    public Vector3 Scale = new(1, 1, 1);
    public bool removeVoid;
    private void Start()
    {
        //FindSpriteRenderers();
    }

    void OnValidate()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = newSprite;
            GameObject currentObject = spriteRenderer.gameObject;
            Collider2DOptimization.PolygonColliderOptimizer script = GetComponent<Collider2DOptimization.PolygonColliderOptimizer>();
            if (script == null)
            {
                currentObject.AddComponent(script.GetType());
            }
            PolygonCollider2D collider;
            if (removeVoid && TryGetComponent(out collider))
                collider.pathCount = 1;

            currentObject.transform.localScale = Scale;

        }
    }
}
