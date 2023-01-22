using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualization : MonoBehaviour
{
    static GameObject clone = null;
    //passed throwable gameobjects must have:
    //RigidBody2D, CircleCollider2d, LineRenderer
    public static void StartVisualizingPath(GameObject throwable)
    {
        //get original object off of the simulation to keep it static
        throwable.GetComponent<Rigidbody2D>().simulated = false;
        throwable.GetComponent<CircleCollider2D>().enabled = false;
        //Create a copy of object to simulate
        clone = Instantiate(throwable, throwable.transform.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().simulated = true;
        clone.GetComponent<CircleCollider2D>().enabled = true;
        //Physics2d has 3 simulation modes which dictates when it simulates Rigidbody2Ds
        //Switch simulationMode from FixedUpdate(default) to Script to simulate with Physics2D.Simulate()
        Physics2D.simulationMode = SimulationMode2D.Script;
    }
    public static void VisualizePath(GameObject throwable, Vector3 force)
    {
        //make sure the path starts at the origin
        clone.transform.position = throwable.transform.position;
        //reset velocity and add the current visualized force
        Rigidbody2D cloneRigidBody = clone.GetComponent<Rigidbody2D>();
        cloneRigidBody.velocity = Vector3.zero;
        cloneRigidBody.AddForce(force);
        //simulate manually as many steps as needed and save position each step
        List<Vector3> pathPoints = new List<Vector3>();
        int simulationSteps = 1000;
        for (int i = 1; i < simulationSteps; i++)
        {
            //using Time.fixedDeltaTime as the time step to match SimulationMode2D.FixedUpdate
            Physics2D.Simulate(Time.fixedDeltaTime);
            pathPoints.Add(cloneRigidBody.transform.position);
        }
        //set stored positions from simulation as linerenderer positions
        LineRenderer linePath = cloneRigidBody.GetComponent<LineRenderer>();
        linePath.enabled = true;
        linePath.positionCount = pathPoints.Count;
        linePath.SetPositions(pathPoints.ToArray());
    }
    public static void StopVisualizingPath(GameObject throwable)
    {
        throwable.GetComponent<CircleCollider2D>().enabled = true;
        throwable.GetComponent<Rigidbody2D>().simulated = true;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        Destroy(clone);
    }
}
