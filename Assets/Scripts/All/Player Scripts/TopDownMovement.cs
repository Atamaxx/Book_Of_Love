using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private float _horizontalInput;
    private float _verticalInput;

    [Range(0, 1)] public float DiagonalMoveLimiter = 0.9f;
    [Range(0, 15)] public float MovementLimiter = 5f;
    public float RunSpeed = 20.0f;
    public float SprintMultiplier = 2;
    public float DecelerationMultiplier = 3;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        DiagonalMovement();
        MovementCalculation();
    }

    private void DiagonalMovement()
    {
        if (_horizontalInput != 0 && _verticalInput != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally
            _horizontalInput *= DiagonalMoveLimiter;
            _verticalInput *= DiagonalMoveLimiter;
        }
    }

    private void MovementCalculation()
    {
        if (Input.GetButton("Sprint"))
            _rigidbody2D.velocity = new Vector2(_horizontalInput * RunSpeed * SprintMultiplier, _verticalInput * RunSpeed * SprintMultiplier);
        else if (Input.GetButton("Deceleration"))
            _rigidbody2D.velocity = new Vector2(_horizontalInput * RunSpeed / DecelerationMultiplier, _verticalInput * RunSpeed / DecelerationMultiplier);
        else
            _rigidbody2D.velocity = new Vector2(_horizontalInput * RunSpeed, _verticalInput * RunSpeed);
    }

    
}
