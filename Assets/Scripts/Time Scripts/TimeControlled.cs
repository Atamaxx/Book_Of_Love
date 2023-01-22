using System.Collections;
using UnityEngine;

public class TimeControlled : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public Vector2 velocity;
    IEnumerator running;


    [SerializeField] private float checkInterval = 0.1f; // The interval at which to check the object's velocity

    private Vector3 previousPosition; // The previous position of the object
    private float timeSinceLastCheck = 0f; // The time since the last velocity check

    private bool current = true;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        running = Run();
        StartCoroutine(running);

    }


    public virtual void TimeUpdate()
    {
        velocity = rb2D.velocity;
    }


    IEnumerator Run()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            //Move your object here
        }
    }
    public void OnPause()
    {
        StopCoroutine(running);
    }

    public void OnResume()
    {
        running = Run();
        StartCoroutine(running);
    }

}


