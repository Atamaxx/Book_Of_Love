using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isCheckpoint = false;
    private bool wasActitated = false;
    private SpriteRenderer sprite;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        ActivateCheckpoint();
    }


    public void ActivateCheckpoint()
    {
        if (wasActitated) return;

        sprite.color = Color.green;
        isCheckpoint = true;
        wasActitated = true;

    }
    public void DeactivateCheckpoint()
    {
        sprite.color = Color.yellow;
        isCheckpoint = false;
    }
}
