using UnityEngine;

public class ObjectSpeed : MonoBehaviour
{
    public float speed; // The current speed of the player
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate the speed by getting the magnitude of the velocity
        speed = rb.velocity.magnitude;
    }
}