using UnityEngine;

public class MoveObjectToCursor : MonoBehaviour
{
    Vector3 mousePosition;
    public float Speed = 1f;
    public float SpeedClamp = 1f;
    public bool Rotation = false;
    public bool IsCursorVisible = false;
    private bool facingRight = true;
    private float currentSpeed = 5f;

    private Vector2 lastObjectPos;
    private Vector2 objectPos;
    private Vector3 clampPos;
    private bool isClamping;

    Rigidbody2D rb2D;

    void Start()
    {
        Cursor.visible = IsCursorVisible;
        lastObjectPos = transform.position;
        currentSpeed = Speed;
        rb2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        StopMovement();
    }

    private void FixedUpdate()
    {

        MoveToCursor();

        if (Rotation)
            RotateObject();
    }

    private void MoveToCursor()
    {
        if (currentSpeed == 0f)
            return;

        Vector3 objPos = transform.position;
        Vector3 moveTo = mousePosition;        

        rb2D.MovePosition(Vector3.Lerp(objPos, moveTo, currentSpeed * UnityEngine.Time.deltaTime));
    }

    private void StopMovement()
    {
        if (Input.GetMouseButtonDown(1) && currentSpeed != 0f)
        {
            currentSpeed = 0f;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            currentSpeed = Speed;
        }
    }

    private void RotateObject()
    {
        Vector2 direction;
        objectPos = transform.position;

        direction = objectPos - lastObjectPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);

        if (mousePosition.x < objectPos.x && facingRight)
        {
            Flip();
        }
        else if (mousePosition.x > objectPos.x && !facingRight)
        {
            Flip();
        }

        lastObjectPos = objectPos;
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;
    }
}
