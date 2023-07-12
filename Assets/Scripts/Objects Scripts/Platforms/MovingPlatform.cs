#if UNITY_EDITOR
using NaughtyAttributes;
#endif

using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float MoveBC = 0f; // DistanceBottomClamp 
    public float MoveTC = 10f; // DistanceTopClamp

    [BoxGroup("LOOP")] public bool toLoop;
    [BoxGroup("LOOP"), ShowIf("toLoop")]
    public float RepeatEach = 10f;



    public void MoveByDistance(List<Vector3> waypoints, float timeDistance, float wayLength)
    {
        if (toLoop)
            MoveDistanceLoop(waypoints, timeDistance, wayLength);
        else
            MoveDistance(waypoints, timeDistance, wayLength);
    }

    void MoveDistance(List<Vector3> waypoints, float timeDistance, float wayLength)
    {
        if (timeDistance <= MoveBC)
            transform.position = waypoints[0];
        else if (timeDistance >= MoveTC)
            transform.position = waypoints[^1];
        else if (MoveBC < timeDistance && timeDistance < MoveTC)
        {
            float platformPercent = Mathf.Clamp((timeDistance % MoveTC - MoveBC) / (MoveTC - MoveBC), 0, 1);

            float platformDist = platformPercent * wayLength;
            transform.position = PositionByPercent(waypoints, platformDist);
        }
    }

    void MoveDistanceLoop(List<Vector3> waypoints, float timeDistance, float wayLength)
    {
        if (timeDistance <= MoveBC)
        {
            transform.position = waypoints[0];
            return;
        }

        float platformPercent;
        if (MoveTC <= timeDistance)
        {
            platformPercent = Mathf.Clamp((MoveTC - MoveBC) % RepeatEach / RepeatEach, 0, 1);
        }
        else// if (MoveBC < timeDistance && timeDistance < MoveTC)
        {
            platformPercent = Mathf.Clamp((timeDistance - MoveBC) % RepeatEach / RepeatEach, 0, 1);
        }

        float platformDist = platformPercent * wayLength;
        transform.position = PositionByPercent(waypoints, platformDist);
    }


    Vector2 PositionByPercent(List<Vector3> waypoints, float platformDist)
    {
        return My.Line.FindPointByLength(waypoints, platformDist);
    }


}
