using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace My
{
    public class Transformations : MonoBehaviour
    {
        #region Moving By Waypoints
        public static void MoveConstant(List<GameObject> waypoints, ref int waypointIndex, Transform objTransform, float speed)
        {
            if (waypoints.Count == 0) return;

            if (Vector2.Distance(waypoints[waypointIndex].transform.position, objTransform.position) < 0.01f)
            {
                waypointIndex++;
                if (waypointIndex >= waypoints.Count)
                {
                    waypointIndex = 0;
                }
            }

            objTransform.position = Vector2.MoveTowards(objTransform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);
        }

        public static void MoveTimeDistance(List<GameObject> waypoints, ref int waypointIndex, Transform objTransform, float speed)
        {

        }

        
        #endregion
    }
}

