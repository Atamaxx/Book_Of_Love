using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewind : MonoBehaviour
{
    public float recordTime = 5f; // time to record for
    public float rewindTime = 3f; // time to rewind

    private List<Vector2> positionList;
    private Rigidbody2D rb;

    private void Start()
    {
        positionList = new List<Vector2>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Record());
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Rewind());
        }
    }

    IEnumerator Record()
    {
        positionList.Clear();
        rb.simulated = false;

        while (positionList.Count < Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            positionList.Add(rb.position);
            yield return new WaitForFixedUpdate();
        }

        rb.simulated = true;
    }

    IEnumerator Rewind()
    {
        rb.simulated = false;

        for (int i = positionList.Count - 1; i >= 0; i--)
        {
            rb.position = positionList[i];
            yield return new WaitForFixedUpdate();

            if (i == 0 || positionList.Count - i > Mathf.Round(rewindTime / Time.fixedDeltaTime))
            {
                break;
            }
        }

        rb.simulated = true;
    }
}
