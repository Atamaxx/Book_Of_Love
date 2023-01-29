using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRigidbodies : MonoBehaviour
{
    Rigidbody2D[] rigidbodies;
    void Start()
    {
        rigidbodies = FindObjectsOfType<Rigidbody2D>();
    }

    public void FreezeAllObjects()
    {
        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            rigidbody.Sleep();
        }
    }

    public void UnFreezeAllObjects()
    {
        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            rigidbody.WakeUp();
        }
    }
}
