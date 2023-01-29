using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float speed = 1.0f;


    private float horizontalInput;
    private float horizontalSpeed;
    private float verticalInput;
    private float verticalSpeed;
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("HorizontalCamera");
        verticalInput = Input.GetAxisRaw("VerticalCamera");
    }

    private void FixedUpdate()
    {
        
        transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0);
    }
}