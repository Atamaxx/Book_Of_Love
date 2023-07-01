using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public LayerMask layerMask; // Now it's a LayerMask, set this in the Inspector

    private Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.AddForce(movement * speed);
    }
    private Vector2 lastSafePosition;

    void OnTriggerStay2D(Collider2D other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            lastSafePosition = transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            transform.position = lastSafePosition;
        }
    }
}
