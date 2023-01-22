using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    private Vector3 throwVector;

    private Rigidbody2D _rb;
    private Collider2D _collider2d;

    [SerializeField] private float throwPower = 200f;
    [SerializeField] private int maxNumberOfThrowingObjects = 5;
    [SerializeField] private GameObject ObjectToThrow;

    [SerializeField] private float yOffset = 2f;
    private Vector3 mousePos;

    void Awake()
    {
        _collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Throw();
        DrawAimLine();
    }


    void CalculateThrowVector()
    {
        
        //doing vector2 math to ignore the z values in our distance.
        Vector2 distance = mousePos - transform.position;
        //dont normalize the ditance if you want the throw strength to vary
        throwVector = distance.normalized * throwPower;
    }

    

    public void Throw()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateObjectToThrow();
            CalculateThrowVector();
            _rb.AddForce(throwVector);
        }
    }

    private void DrawAimLine()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(transform.position, mousePos, Color.yellow);
    }

    private void CreateObjectToThrow()
    {
        Vector3 objPosition = new(transform.position.x, transform.position.y + yOffset, transform.position.z);
        GameObject obj = Instantiate(ObjectToThrow, objPosition, Quaternion.identity);
        _rb = obj.GetComponent<Rigidbody2D>();
        IgnorePlayerCollider(obj);
    }

    private void IgnorePlayerCollider(GameObject obj)
    {
        Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), _collider2d);
    }

    private void DestroyExtraObjects()
    {
        
    }
}
