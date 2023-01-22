using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collisionCollider = collision.collider;
        if (collisionCollider.CompareTag("Player"))
        {
            Destroy(collisionCollider.gameObject);
        }
    }
}
