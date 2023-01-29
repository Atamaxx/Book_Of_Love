using UnityEngine;

public class MoveObjectToCursor : MonoBehaviour
{

    Vector3 mousePosition;
    public float speed = 5f;
    private bool facingRight = true;

    private Vector2 lastObjectPos;
    private Vector2 objectPos;

    void Start()
    {
        Cursor.visible = false;
        lastObjectPos = transform.position;

    }
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, mousePosition, speed * Time.deltaTime);
        RotateObject(); 
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
