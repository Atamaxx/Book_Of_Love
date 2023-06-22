using UnityEngine;

public class BoundsModifier : MonoBehaviour
{
    public Transform ToGuard;
    public Transform GuardFrom;

    private Vector2 cornerPos;
    public float cornerPos1;
    public float cornerPos2;

    private SpriteRenderer spriteRenderer;

    public Vector2 point1;
    public Vector2 point2;
    public Vector2 point3;
    public Vector2 point4;

    //private Rigidbody2D rb2D;
    void Start()
    {
        SetUp();
        GuardianMode();
    }

    private void SetUp()
    {
        // Create a game object representing the box
        spriteRenderer = GetComponent<SpriteRenderer>();
        //rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cornerPos = GuardFrom.position;
        GuardianMode();
    }
    void DanceGuardianMode()
    {
        if (GuardFrom != null)
        {
            print("SDAS");
            // point1 = spriteRenderer.bounds.min;
            point2 = new(cornerPos.x, point1.y);
            point3 = new(cornerPos.x, cornerPos.y);
            point4 = new(point1.x, cornerPos.y);
        }
        // Calculate the minimum and maximum x and y values
        float minX = Mathf.Min(point1.x, point2.x, point3.x, point4.x);
        float maxX = Mathf.Max(point1.x, point2.x, point3.x, point4.x);
        float minY = Mathf.Min(point1.y, point2.y, point3.y, point4.y);
        float maxY = Mathf.Max(point1.y, point2.y, point3.y, point4.y);

        // Calculate the width and height of the box
        float width = maxX - minX;
        float height = maxY - minY;

        // Set the size of the box sprite
        transform.localScale = new(width * cornerPos1, height * cornerPos2);

        // Set the position of the box
        float centerX = (minX + maxX) / 2f;
        float centerY = (minY + maxY) / 2f;
        gameObject.transform.position = new Vector3(centerX, centerY, 0f);

        // Optionally, rotate the box to align with the points
        Vector3 lookDir = new Vector3(point2.x - point1.x, point2.y - point1.y, 0f);
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir);
    }

    public float duration = 1f;

    private float elapsedTime = 0f;

    private void GuardianMode()
    {
        Vector2 startingPoint = ToGuard.position;
        Vector2 endPoint = GuardFrom.position;
        float distance = Vector2.Distance(startingPoint, endPoint);
        RaycastHit2D hit = Physics2D.Raycast(startingPoint, (endPoint - startingPoint).normalized, distance, Info.PlatformLayer);
        Debug.DrawRay(startingPoint, endPoint - startingPoint, Color.red);

        if (hit)
        {
            print("1");
            //StartCoroutine(My.Move.MoveObject(transform, startingPoint, 5f));
            elapsedTime += Time.deltaTime;

            if (elapsedTime > duration)
            {
                elapsedTime = duration;
            }

            // Calculate interpolation factor based on elapsed time and duration
            float t = elapsedTime / duration;

            // Interpolate between the start and end points
            transform.position = Vector3.Lerp(transform.position, startingPoint, t);
        }
        else
        {
            print("2");
            transform.position = (startingPoint + endPoint) / 2;
            //StartCoroutine(My.Move.MoveObject(transform, (startingPoint + endPoint) / 2, 5f));

            //My.Move.ToTarget(transform, transform.position, (startingPoint + endPoint) / 2, 5f);
        }
    }


}

