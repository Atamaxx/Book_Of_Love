using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XDisapear : MonoBehaviour
{
    public Transform DestroyerTransform;
    private float _destroyerX;
    private float _leftX;
    private float _rightX;
    private Material _material;
    // Start is called before the first frame update
    void Start()
    {
        // Get the object's renderer component
        Renderer objectRenderer = gameObject.GetComponent<Renderer>();

        // Get the object's bounding box
        Bounds objectBounds = objectRenderer.bounds;
        _material = GetComponent<SpriteRenderer>().material;
        // Calculate the left and right x coordinates
        _leftX = objectBounds.center.x - objectBounds.size.x / 2f;
        _rightX = objectBounds.center.x + objectBounds.size.x / 2f;

    }

    // Update is called once per frame
    void Update()
    {
        _destroyerX = DestroyerTransform.position.x;
        if (_destroyerX < _leftX)
        {
            _material.SetFloat("_xDestroy", 0);
        }
        else if (_destroyerX > _rightX)
        {
            _material.SetFloat("_xDestroy", 1);
        }
        else if (_destroyerX >= _leftX && _destroyerX <= _rightX)
        {
            _material.SetFloat("_xDestroy", (_destroyerX - _leftX) / (_rightX - _leftX));
        }


    }
}
