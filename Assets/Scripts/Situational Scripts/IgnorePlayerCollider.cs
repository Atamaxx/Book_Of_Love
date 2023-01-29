using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCollider : MonoBehaviour
{
    [SerializeField] private Collider2D playerCollider;

    private void Awake()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
    }
}
