using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
//[RequireComponent(typeof(LineRenderer))]
public class MovingTransform : MonoBehaviour
{
    [SerializeField] private BookOf.Time _timeLine;


    [BoxGroup("MOVE BY DISTANCE")] public bool toMoveByDistance;
    [BoxGroup("MOVE BY DISTANCE"), ShowIf("toMoveByDistance")]
    public float MoveBC = 0f; // DistanceBottomClamp 
    [ShowIf("toMoveByDistance"), BoxGroup("MOVE BY DISTANCE")]
    public float MoveTC = 10f; // DistanceTopClamp


    [BoxGroup("ROTATION")]
    public bool toRotateByDistance;
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public bool isClockwise = false;
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")] 
    public float RotateBC = 0f; // RotateBottomClamp
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")]
    public float RotateTC = 10f; // RotateTopClamp
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")] 
    public float RotateMin = 0f; // RotateTopClamp
    [BoxGroup("ROTATION"), ShowIf("toRotateByDistance")] 
    public float RotateMax = 180f; // RotateTopClamp


    [BoxGroup("MOVE BY PERCENT")]
    public bool toMoveByPercent = false;
    [BoxGroup("MOVE BY PERCENT"), ShowIf("toMoveByPercent")] [Range(0, 2)] public float PercentClamp = 1f;


    [BoxGroup("100%")]
    public bool EndAction = false;
    [BoxGroup("100%"), ShowIf("EndAction")]
    public float DestroyDelay = 1.5f;


    private LineRenderer _timeLR;
    readonly List<Vector3> _linePoints = new();

    private int _numberOfVertices;
    private float _distancePassed;

    private void Start()
    {
        if (!toMoveByDistance && !toMoveByPercent) return;
        
        SetUp();
    }
    private void SetUp()
    {
        _timeLR = GetComponent<LineRenderer>();

        _numberOfVertices = _timeLR.positionCount;

        for (int i = 0; i < _numberOfVertices; i++)
        {
            _linePoints.Add(_timeLR.GetPosition(i));
        }

    }


    private void Update()
    {
        if (!toMoveByDistance && !toMoveByPercent && !toRotateByDistance) return;


        _distancePassed = _timeLine.ÑurrentLength;

        if (toMoveByDistance)
        {
            MoveByDistance();
        }
        else if (toMoveByPercent)
        {
            MoveByPercent();
        }

        if (toRotateByDistance)
        {
            RotateByDistance();
        }

    }

    void MoveByDistance()
    {
        if (MoveBC > _distancePassed)
        {
            transform.position = _linePoints[0];
        }
        else if (_distancePassed > MoveTC)
        {
            transform.position = _linePoints[^1];

            if (EndAction)
            {
                Destroy(gameObject, DestroyDelay);
            }
        }
        else if (MoveBC < _distancePassed && _distancePassed < MoveTC)
        {
            //float clamp = MoveTC - MoveBC;
            //_percentPassed = Mathf.Clamp((_distancePassed % MoveTC - MoveBC) / clamp, 0, 1);
            transform.position = PositionByPercent();
        }
    }
    void MoveByPercent()
    {
        //_percentPassed = Mathf.Clamp(_timeLine.PercentPassed / PercentClamp, 0.01f, 1);
        transform.position = PositionByPercent();
    }

    void RotateByDistance()
    {
        float rotationValue;
        float distanceValue = Mathf.Clamp(_distancePassed, RotateBC, RotateTC);
        rotationValue = Mathf.Lerp(RotateMin, RotateMax, (distanceValue - RotateBC) / (RotateTC - RotateBC));
        Vector3 targetRotation;
        if (isClockwise)
        {
            targetRotation = new(0f, 0f, -rotationValue);
        }
        else
        {
            targetRotation = new(0f, 0f, rotationValue);
        }

        transform.rotation = Quaternion.Euler(targetRotation);
    }


    Vector2 PositionByPercent()
    {
        return My.Line.FindPointByLength(_linePoints, _distancePassed);
    }
}

