using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public Collider2D playerCollider;
    public Rigidbody2D rigidbody2D;

    private float dashTimer;
    private bool isDashing = false;

    [SerializeField] private Transform wallCheck;
    private Vector2 wallCheckPosition;


    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        wallCheckPosition = wallCheck.position;
    }
    

    public void Method1()
    {
        if (Input.GetButtonDown("Dash"))
        {
            isDashing = true;
            dashTimer = dashDuration;
            playerCollider.isTrigger = true;
            rigidbody2D.isKinematic = true;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                playerCollider.isTrigger = false;
                rigidbody2D.isKinematic = false;
            }

            transform.position += dashSpeed * Time.deltaTime * transform.right;
        }
    }

    public bool DashThoughWalls(LayerMask platformLayer)
    {
        wallCheckPosition = wallCheck.position;
        float playersHeight = playerCollider.bounds.size.y;
        float playersWidth = playerCollider.bounds.size.x;

        RaycastHit2D hitDown = Physics2D.Raycast(wallCheckPosition, Vector2.down, playersHeight / 2, platformLayer);
        RaycastHit2D hitUp = Physics2D.Raycast(wallCheckPosition, Vector2.up, playersHeight / 2, platformLayer);
        RaycastHit2D hitlLeft = Physics2D.Raycast(wallCheckPosition, Vector2.left, playersWidth / 2, platformLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(wallCheckPosition, Vector2.right, playersWidth / 2, platformLayer);
        float facDirection = transform.localScale.x;
        Vector2 facingDirection;
        if (facDirection > 0)
        {
            facingDirection = Vector2.right;
        }
        else
        {
            facingDirection = Vector2.left;
        }

        RaycastHit2D wallHit = Physics2D.Raycast(wallCheckPosition, -facingDirection, Vector2.Distance(wallCheckPosition, transform.position), platformLayer);

        Debug.DrawRay(wallCheckPosition,(playersHeight / 2) * Vector2.down, Color.blue, 5);
        Debug.DrawRay(wallCheckPosition, (playersHeight / 2) * Vector2.up, Color.blue, 5);
        Debug.DrawRay(wallCheckPosition, (playersWidth / 2) * Vector2.left, Color.blue, 5);
        Debug.DrawRay(wallCheckPosition, (playersWidth / 2) * Vector2.right, Color.blue, 5);

        Debug.DrawRay(wallCheckPosition, (Vector2.Distance(wallCheckPosition, transform.position)) * -facingDirection, Color.cyan, 0.1f);

        if (wallHit.collider == null)
        {
            return false;
        }

        if (hitDown.collider == null && hitUp.collider == null && hitlLeft.collider == null && hitRight.collider == null)
        {
            transform.position = wallCheck.position;
            return true;
        }
        return false;
    }

}
