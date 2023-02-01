using System.Collections.Generic;
using UnityEngine;

public class ControlRigidbodies : MonoBehaviour
{
    public Rigidbody2D[] rigidbodies;
    public List<RigidbodyConstraints2D> originalConstraints = new();

    void Awake()
    {
        rigidbodies = FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            originalConstraints.Add(rigidbody.constraints);
        }
    }

    public void FreezeAllObjects()
    {
        foreach (Rigidbody2D rigidbody in rigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void UnFreezeAllObjects()
    {
        for (int i = 0; i < originalConstraints.Count; i++)
        {
            rigidbodies[i].constraints = originalConstraints[i];
        }
    }
}
