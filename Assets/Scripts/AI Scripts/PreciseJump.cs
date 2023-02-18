using UnityEngine;
public class PreciseJump : MonoBehaviour
{
    // public Transform point1;
    public Transform point2;
    public float minAngle = 30f;
    public float maxAngle = 90f;

    public Rigidbody2D rigidbody;
    public float V;
    public float jumpForce;

    private float _gravity;

    private void Start()
    {
        _gravity = Physics.gravity.y;
    }


    void Update()
    {
        Jump1();
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Jump();
        //}
    }

    public void Jump()
    {
        rigidbody.velocity = CalculateForce(CalculateAngle());
    }
    public float _distance;
    public float _midForce;
    private Vector2 CalculateForce(float angle)
    {
       // _distance = Vector2.Distance(point2.position, transform.position);

        _distance = point2.position.x - transform.position.x;
        float h = point2.position.y - transform.position.y;
        float angleRad = angle * Mathf.Deg2Rad;
        
        //v = sqrt((d ^ 2 * g) / (2 * cos ^ 2(?) * (d * tan(?) - h)))
        V = Mathf.Sqrt(Mathf.Abs(_distance * _distance * _gravity / (2 * Mathf.Cos(angleRad) * Mathf.Cos(angleRad) * (_distance * Mathf.Tan(angleRad) - h))) );


        //V = Mathf.Sqrt(Mathf.Abs(_distance * Physics.gravity.y / Mathf.Sin(2 * angleRadians)));

        float V_horizontal = V * Mathf.Cos(angleRad);
        float V_vertical = V * Mathf.Sin(angleRad);


        return new Vector2(V_horizontal, V_vertical);
    }


    private float CalculateAngle()
    {
        // Find the direction vector between the two points
        Vector2 direction = point2.position - transform.position;

        // Calculate the angle between the direction vector and the horizontal axis
        float currentAngle = Vector2.Angle(Vector2.right, direction);
        // Interpolate the angle based on the y position of point1
        float jumpAngle = (1 - minAngle / maxAngle) * currentAngle + minAngle;

        return jumpAngle;
    }

    public float currentAngle;
    private void Jump1()
    {
        // Check for a mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Find the mouse position in world space
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Calculate the direction to the mouse position
            Vector2 direction = (worldPosition - (Vector2)transform.position).normalized;

            currentAngle = Vector2.Angle(Vector2.right, direction);
            // Apply a force in the direction of the mouse position
            rigidbody.AddForce(CalculateForce(currentAngle), ForceMode2D.Impulse);
        }
    }
}


//// Find the direction vector between the two points
//Vector2 direction = point2.position - point1.position;

//// Calculate the angle between the direction vector and the horizontal axis
//currentAngle = Vector2.Angle(Vector2.right, direction);
//print(currentAngle);
//// Interpolate the angle based on the y position of point1
//jumpAngle = (1 - minAngle / maxAngle) * currentAngle + minAngle;
////jumpAngle = Mathf.LerpAngle(minAngle, maxAngle, t);

//// Rotate the object to face the direction vector with the interpolated angle
//transform.rotation = Quaternion.Euler(0f, 0f, jumpAngle);