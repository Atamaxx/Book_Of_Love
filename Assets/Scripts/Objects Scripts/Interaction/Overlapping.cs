using UnityEngine;

public class Overlapping : MonoBehaviour
{
    // Update is called once per frame

    public float CurrentOverlap;
    public float OverlapThreshold = 90f;
    public bool IsOverlaping;
    private BoxCollider2D _boxCollider2D;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Get all overlapping colliders
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(
            _boxCollider2D.bounds.center,
            _boxCollider2D.bounds.size,
            0f
        );

        // Calculate the overlapping percentage with each overlapping collider
        foreach (Collider2D collider in overlappingColliders)
        {
            BoxCollider2D otherBoxCollider2D = collider as BoxCollider2D;

            if (otherBoxCollider2D != null && otherBoxCollider2D != _boxCollider2D)
            {
                CurrentOverlap = CalculateOverlappingPercentage(_boxCollider2D, otherBoxCollider2D);
            }
        }

        IsOverlaping = Overlap();
    }

    private bool Overlap()
    {
        if (CurrentOverlap > OverlapThreshold) return true;

        return false;
    }

    private float CalculateOverlappingPercentage(BoxCollider2D box1, BoxCollider2D box2)
    {
        // Calculate the overlap box
        float minX = Mathf.Max(box1.bounds.min.x, box2.bounds.min.x);
        float minY = Mathf.Max(box1.bounds.min.y, box2.bounds.min.y);
        float maxX = Mathf.Min(box1.bounds.max.x, box2.bounds.max.x);
        float maxY = Mathf.Min(box1.bounds.max.y, box2.bounds.max.y);

        // Calculate the width and height
        float width = maxX - minX;
        float height = maxY - minY;

        if (width < 0f || height < 0f)
        {
            // The boxes do not overlap
            return 0f;
        }

        float overlapArea = width * height;
        float box1Area = box1.bounds.size.x * box1.bounds.size.y;

        // Calculate the percentage
        float percentage = overlapArea / box1Area * 100f;
        return percentage;
    }
}
