using System.Collections;
using UnityEngine;

public class TimeControlled : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public Vector2 velocity;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }


    public virtual void TimeUpdate()
    {
        rb2D.velocity = Vector2.zero;
    }


}


