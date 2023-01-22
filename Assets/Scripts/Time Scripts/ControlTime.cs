using System.Collections.Generic;
using UnityEngine;

public class ControlTime : MonoBehaviour
{
    float timeSpeed;

    Rigidbody2D rb;

    private bool stepBack;
    private bool pause;
    private bool stepForward;

    public struct RecordedData
    {
        public Vector2 position;
        public Vector2 velocity;
    }

    RecordedData[,] recordedData;

    int recordLimitation = 100000;
    int recordCount;
    int recordIndex = 100000;

    bool wasSteppingBack = false;
    bool isPaused = false;

    TimeControlled[] timeObjects;
    int numberOfTimeObjects;
    public List<TimeControlled> keepRunningScripts;


    private void Awake()
    {
        timeObjects = FindObjectsOfType<TimeControlled>();
        numberOfTimeObjects = timeObjects.Length;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        recordedData = new RecordedData[numberOfTimeObjects, recordLimitation];

    }

    void Update()
    {
        stepBack = Input.GetButton("Rewind");
        pause = Input.GetKeyDown(KeyCode.F);
        stepForward = Input.GetKeyDown(KeyCode.E);

        Pause();

    }


    private void FixedUpdate()
    {
        Rewind();
        FastForward();
        NoTimeManipulation();
    }

    private void Rewind()
    {
        if (!stepBack) return;

        if (recordIndex <= 0) return;

        wasSteppingBack = true;
        recordIndex--;

        for (int objectIndex = 0; objectIndex < numberOfTimeObjects; objectIndex++)
        {
            TimeControlled timeObject = timeObjects[objectIndex];
            RecordedData data = recordedData[objectIndex, recordIndex];
            timeObject.transform.position = data.position;
            timeObject.velocity = data.velocity;
        }
    }

    private void FastForward()
    {
        if (pause && stepForward)
        {
            wasSteppingBack = true;
            if (recordIndex < recordCount - 1)
            {
                recordIndex++;
                for (int objectIndex = 0; objectIndex < numberOfTimeObjects; objectIndex++)
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedData[objectIndex, recordIndex];
                    timeObject.transform.position = data.position;
                    timeObject.velocity = data.velocity;
                }
            }
        }
    }




    private void NoTimeManipulation()
    {
        if (wasSteppingBack)
        {
            recordCount = recordIndex;
            wasSteppingBack = false;
            return;
        }

        if (pause || stepBack) return;


        for (int objectIndex = 0; objectIndex < numberOfTimeObjects; objectIndex++)
        {
            TimeControlled timeObject = timeObjects[objectIndex];
            RecordedData data = new()
            {
                position = timeObject.transform.position,
                velocity = timeObject.velocity
            };
            recordedData[objectIndex, recordCount] = data;
        }
        recordCount++;
        recordIndex = recordCount;

        foreach (TimeControlled timeObject in timeObjects)
        {
            timeObject.TimeUpdate();
        }

    }

    private void Pause()
    {
        if (!pause) return;

        if (!isPaused)
        {
            
            Debug.Log("AA");
            Time.timeScale = 0;
            isPaused = true;
            foreach (TimeControlled script in keepRunningScripts)
            {
                script.enabled = true;
            }
        }
        else if (isPaused)
        {

            Debug.Log("BB");

            Time.timeScale = 1;
            foreach (TimeControlled script in keepRunningScripts)
            {
                script.enabled = false;
            }
            isPaused = false;
        }
    }

    private void TimePauseExceptions()
    {
        GameObject objectExeption;
        objectExeption = GameObject.Find("Main Player");
        objectExeption.SetActive(true);
    }

    private void AfterSteppingBack()
    {
        if (wasSteppingBack)
        {
            recordCount = recordIndex;
            wasSteppingBack = false;


        }
    }
}
