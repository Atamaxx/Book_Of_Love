using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private float _lineOffset;
    [SerializeField] private Transform _target;
    [SerializeField] private PolygonCollider2D _sightLine;
    [SerializeField] private LayerMask _targetAndGroundLayer;
    //[field: SerializeField] public bool TargetDetected { get; private set; }
    [field: SerializeField] public bool SearchingEnemy { get; private set; }
    [field: SerializeField] public bool IsTargetRight { get; private set; }
    public bool TargetDetected;
    [field: SerializeField] public EnemyData Stats { get; private set; }

    private void Awake()
    {
        _sightLine = GetComponent<PolygonCollider2D>();
    }


    private void Start()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(0f, 0f),
            new Vector2(-Stats.DetectRange, _lineOffset),
            new Vector2(-Stats.DetectRange, -_lineOffset)
        };

        _sightLine.SetPath(0, points);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_target.tag))
        {
            float distance = Vector2.Distance(transform.position, _target.position);
            Vector2 direction = (_target.position - transform.position).normalized;
            RaycastHit2D hitTarget = Physics2D.Raycast(transform.position, direction, distance, _targetAndGroundLayer);
            if (hitTarget.collider == null || !hitTarget.collider.CompareTag(_target.tag)) return;

            TargetDetected = true;
            SearchingEnemy = false;

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_target.tag))
        {
            //TargetDetected = false;
            SearchingEnemy = true;
        }
    }

//    private void OnDrawGizmos()
//    {
//        Debug.DrawLine(transform.position, _target.position, Color.blue);
//    }
}
