using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] [Range(0,100)] private float jumpForce = 10f;
    private Rigidbody2D playerRb2d;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb2d = other.GetComponent<Rigidbody2D>();
            playerRb2d.velocity = Vector2.up * jumpForce;
        }
    }
}