using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreLayer : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToIgnore;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, _layersToIgnore, true);
    }
}
