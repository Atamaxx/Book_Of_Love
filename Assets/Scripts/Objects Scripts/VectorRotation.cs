using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VectorRotation : MonoBehaviour
{
    private Transform objectToRotate;
    private Vector2 lastObjectPos;
    private Vector2 objectPos;

    private void Start()
    {
        lastObjectPos = transform.position;
    }

    private void FixedUpdate()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        Vector2 direction;
       // float localScaleX = transform.localScale.x;
        objectPos = transform.position;

        direction =  objectPos - lastObjectPos;

        //if (direction.x < -0.05) transform.localScale = new(-localScaleX, transform.localScale.y, transform.localScale.z);
        //else transform.localScale = new(localScaleX, transform.localScale.y, transform.localScale.z);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);

        lastObjectPos = objectPos;

        Debug.Log(direction);
    }

}
