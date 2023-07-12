using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    public float Speed = 10f;


    public void ConstantMove(List<Vector3> waypoints, ref int waypointIndex)
    {
        if (waypoints.Count == 0) return;

        if (Vector2.Distance(waypoints[waypointIndex], transform.position) < 0.01f)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex], Speed * Time.deltaTime);
    }
}
