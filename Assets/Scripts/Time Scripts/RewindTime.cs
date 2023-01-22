using System.Collections.Generic;
using UnityEngine;

public class RewindTime : MonoBehaviour
{
    // A flag to indicate whether or not the rewind is currently active
    private bool rewinding;


    void Update()
    {
        // If the rewind button is pressed
        if (Input.GetButtonDown("Rewind"))
        {
            rewinding = true;
        }
        if (Input.GetButtonUp("Rewind"))
        {
            rewinding = false;
        }

    }


    private void RewindTransform()
    {
        List<Vector3> positions;
        positions = new List<Vector3>();

        if (rewinding)
        {
            // Remove the last position from the list and set the object's position to it
            if (positions.Count > 0)
            {
                transform.position = positions[positions.Count - 1];
                positions.RemoveAt(positions.Count - 1);
            }
            else
            {
                // If there are no more positions, stop rewinding
                rewinding = false;
            }
        }
        else
        {
            // If rewinding is not active, add the current position to the list
            positions.Add(transform.position);
        }
    }


}
