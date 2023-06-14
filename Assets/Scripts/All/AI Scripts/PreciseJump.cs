using UnityEngine;
public class PreciseJump : MonoBehaviour
{
    [SerializeField] private float _minAngle = 30f;
    [SerializeField] private float _maxAngle = 90f;

    private Vector2 point1;
    private Vector2 point2;

    private float _gravity;

    private void Start()
    {
        _gravity = Physics.gravity.y;
    }

    public void SetUpJump(Rigidbody2D rb2D, Vector2 start, Vector2 end)
    {
        point1 = start;
        point2 = end;
        _gravity = Physics.gravity.y * rb2D.mass * rb2D.gravityScale;
    }

    public Vector2 CalculateVelocity()
    {
        float angle = CalculateAngle();
        //Horizontal distance between point 1 and point 2
        float d = point2.x - point1.x;
        //Vertical distance between point 1 and point 2
        float h = point2.y - point1.y;
        //Launch angle in radians
        float angleRad = angle * Mathf.Deg2Rad;

        //Calculate velocity needed in oder to reach point two
        float V = Mathf.Sqrt(Mathf.Abs(d * d * _gravity / (2 * Mathf.Cos(angleRad) * Mathf.Cos(angleRad) 
            * (d * Mathf.Tan(angleRad) - h))));

        //Convert velocity into horizontal and vertical velocity
        float V_horizontal = V * Mathf.Cos(angleRad);
        float V_vertical = V * Mathf.Sin(angleRad);

        return new Vector2(V_horizontal, V_vertical);
    }


    private float CalculateAngle()
    {
        // Find the direction vector between the two points
        Vector2 direction = point2 - point1;

        // Calculate the angle between the direction vector and the horizontal axis
        float currentAngle = Vector2.Angle(Vector2.right, direction);
        // Interpolate the angle based on the y position of point1
        float jumpAngle = (1 - _minAngle / _maxAngle) * currentAngle + _minAngle;

        return jumpAngle;
    }
}


//private void JumpToMouse()
//{
//    // Find the mouse position in world space
//    Vector3 mousePosition = Input.mousePosition;
//    mousePosition.z = -Camera.main.transform.z;
//    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

//    // Calculate the direction to the mouse position
//    Vector2 direction = (worldPosition - (Vector2)point1).normalized;

//    float currentAngle = Vector2.Angle(Vector2.right, direction);
//    // Apply a force in the direction of the mouse position
//    _rb2D.AddForce(CalculateVelocity(currentAngle), ForceMode2D.Impulse);
//}