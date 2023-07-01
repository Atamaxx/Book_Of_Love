using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownInHole : MonoBehaviour
{
    [Header("SETTINGS")]
    public bool SunUp = false;
    public float SqueezeRatio = 2;


    private Collider2D _coll2D;
    private Material _material;
    public Transform _teleportPoint;
    public Time _time;
    private AshesToAshes _ashes;


    private void Start()
    {
       // Move.OnCoroutineEnd += HandleCoroutineEnd;
        _coll2D = GetComponent<Collider2D>();
        _ashes = transform.parent.gameObject.GetComponent<AshesToAshes>();
        if (!SunUp) SqueezeCollider();

        _material = GetComponent<Renderer>().material;
    }
    private void OnDestroy()
    {
        //Move.OnCoroutineEnd -= HandleCoroutineEnd;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (SunUp)
            {
                Dusk();
            }
            else if (!SunUp)
            {
                Dawn();
            }
        }
    }

    private void Dawn()
    {
        SunUp = true;
        UpscaleCollider();
        _coll2D.isTrigger = false;
        FlipMaterial();
    }

    private void Dusk()
    {
        SunUp = false;

        SqueezeCollider();
        _coll2D.isTrigger = true;
        FlipMaterial();
    }

    void FlipMaterial()
    {
        int flip = 1 - _material.GetInt("_Flip");
        _material.SetInt("_Flip", flip);
    }

    void SqueezeCollider()
    {
        if (_coll2D is BoxCollider2D boxCollider)
        {
            boxCollider.size /= SqueezeRatio;
        }
        else if (_coll2D is CircleCollider2D circleCollider)
        {
            circleCollider.radius /= SqueezeRatio;
        }
    }

    void UpscaleCollider()
    {
        if (_coll2D is BoxCollider2D boxCollider)
        {
            boxCollider.size *= SqueezeRatio;
        }
        else if (_coll2D is CircleCollider2D circleCollider)
        {
            circleCollider.radius *= SqueezeRatio;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!SunUp && collision.CompareTag("Player"))
    //    {
    //        if (!_ashes.IsTeleporting && Move.IsEnded())
    //        {
    //            _ashes.IsTeleporting = true;
    //        }
    //        else if (!Move.IsEnded())
    //        {
    //            _ashes.IsTeleporting = false;
    //        }
    //    }
        
    //}

    //private void HandleCoroutineEnd()
    //{
    //    // Perform actions when the Coroutine ends
    //    Debug.Log("Coroutine ended");
    //}
}
