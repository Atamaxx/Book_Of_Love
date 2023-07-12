using UnityEngine;

#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public class RotatingPlatform : MonoBehaviour
{
    public bool isClockwise = false;
    public float RotateBC = 0f; // RotateBottomClamp
    public float RotateTC = 10f; // RotateTopClamp
    public float RotateMin = 0f; // RotateTopClamp
    public float RotateMax = 180f; // RotateTopClamp

    [BoxGroup("LOOP")] public bool toLoop;
    [BoxGroup("LOOP"), ShowIf("toLoop")]
    public float RepeatEach = 10f;
    [HideInInspector] public float g_angle;



    public void RotateByDistance(float timeDistance)
    {
        if (toLoop)
            RotateDistanceLoop(timeDistance);
        else
            RotateDistance(timeDistance);
    }


    private void RotateDistance(float timeDistance)
    {
        float rotationValue;
        float distanceValue = Mathf.Clamp(timeDistance, RotateBC, RotateTC);
        rotationValue = Mathf.Lerp(RotateMin, RotateMax, (distanceValue - RotateBC) / (RotateTC - RotateBC));
        Vector3 targetRotation;

        if (isClockwise)
            rotationValue = -rotationValue;

        g_angle = rotationValue;

        targetRotation = new(0f, 0f, rotationValue);

        transform.rotation = Quaternion.Euler(targetRotation);
    }

    private void RotateDistanceLoop(float timeDistance)
    {
        float rotationValue;
        float distanceValue = Mathf.Clamp(timeDistance, RotateBC, RotateTC);
        rotationValue = Mathf.Lerp(RotateMin, RotateMax, (distanceValue - RotateBC) % RepeatEach / RepeatEach);

        Vector3 targetRotation;

        if (isClockwise)
            rotationValue = -rotationValue;

        g_angle = rotationValue;

        targetRotation = new(0f, 0f, rotationValue);

        transform.rotation = Quaternion.Euler(targetRotation);
    }
}
