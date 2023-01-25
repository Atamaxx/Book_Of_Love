using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemy : MonoBehaviour
{
    [SerializeField] private Gameplay gameplay;

    private Vector2 spawnPoint;


    private void Start()
    {
        SpawnEnemy();
    }


    private void SpawnEnemy()
    {
        transform.position = gameplay.FindPointOnCollider(transform.position);
    }
}
