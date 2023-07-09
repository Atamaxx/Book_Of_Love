using UnityEngine;

public class MoveObjectToCursor : MonoBehaviour
{
    //Vector3 mousePosition;
    public float Speed = 1f;
    public float SpeedClamp = 1f;
    public bool IsCursorVisible = false;
    public bool MoveInsideLayer = false;
    public float currentSpeed = 1f;
    public float radius = 0.5f; // Radius of the overlap circle, adjust as needed


    private Vector2 _objectPos;
    private Vector2 _moveTo;

    public bool _isMovementStoped;
    Rigidbody2D rb2D;

    void Start()
    {
        Cursor.visible = IsCursorVisible;
        currentSpeed = Speed;
        rb2D = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        StopMovement();

        _objectPos = transform.position;
        _moveTo = mousePosition;

        if (_isMovementStoped) return;

        if (MoveInsideLayer && !CircleHit())
        {
            currentSpeed = -0.01f;
            //rb2D.MovePosition(lastSafePosition);
        }
        else
        {
            currentSpeed = Speed;
        }

        MoveToCursor();

    }

    private void MoveToCursor()
    {
        rb2D.MovePosition(Vector3.Lerp(_objectPos, _moveTo, currentSpeed * Time.deltaTime));
    }

    private void StopMovement()
    {
        if (InputManager.RightMouseButtonDown && currentSpeed != 0f)
        {
            currentSpeed = 0f;
            _isMovementStoped = true;
        }
        else if (InputManager.RightMouseButtonDown)
        {
            currentSpeed = Speed;
        _isMovementStoped = false;
        }

    }




    private LayerMask MovementLayer => Info.PlatformLayer;

    private Vector2 lastSafePosition;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!MoveInsideLayer) return;
        if (MovementLayer == (MovementLayer | (1 << other.gameObject.layer)))
        {
            lastSafePosition = transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!MoveInsideLayer) return;

        if (MovementLayer == (MovementLayer | (1 << other.gameObject.layer)))
        {
            rb2D.MovePosition(lastSafePosition);
            // transform.position = lastSafePosition;
        }
    }
    public float cirlceOffset = 2f;
    Vector2 cirlceCenter;
    bool CircleHit()
    {
        Vector2 direction = (_moveTo - _objectPos).normalized;

        cirlceCenter = _objectPos + direction * cirlceOffset;
        Collider2D hitColliders = Physics2D.OverlapCircle(cirlceCenter, radius, MovementLayer);

        if (hitColliders)
        {
            return true;
        }

        return false;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(cirlceCenter, radius);

    }
}
