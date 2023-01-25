using System.Collections.Generic;
using UnityEngine;

public class RewindTime : MonoBehaviour
{
    // A flag to indicate whether or not the rewind is currently active

    private bool stepBack;
    private bool pause;
    private bool stepForward;

    private bool isPaused;

    //private bool rewinding;
    [SerializeField] private float standingThreshold = 0.1f;

    [SerializeField] private List<List<Vector2>> positions = new();

    [SerializeField] private TimeControlled[] timeObjects;
    [SerializeField] private List<GameObject> newTimeObjects;

    private GameObject player;

    int numberOfTimeObjects;

    [SerializeField] private MusicController controlMusic;
    private PlayerMovement movement;



    int recordLimitation = 100000;

    private Rigidbody2D rb;

    public float magnitudee;

    //bool isPaused = false;

    private void Awake()
    {

        timeObjects = FindObjectsOfType<TimeControlled>();

        numberOfTimeObjects = timeObjects.Length;
        newTimeObjects = new List<GameObject>();


    }
    private void Start()
    {
        
        foreach (TimeControlled timeObject in timeObjects)
        {

            newTimeObjects.Add(timeObject.gameObject);
            if (timeObject.CompareTag("Player"))
            {
                player = timeObject.gameObject;
                rb = player.GetComponent<Rigidbody2D>();
            }
            positions.Add(new List<Vector2>());
        }

        movement = player.GetComponent<PlayerMovement>();



    }
    void Update()
    {
        stepBack = Input.GetButton("Rewind");
        pause = Input.GetKeyDown(KeyCode.F);
        stepForward = Input.GetKeyDown(KeyCode.D);

        PauseResumeGame();

    }

    private void FixedUpdate()
    {

        AllTimeOperations();
    }



    private void AllTimeOperations()
    {
        int index = 0;

        foreach (GameObject timeObject in newTimeObjects)
        {
            Debug.Log(timeObject);
            RecordPositions(timeObject, index);
            RewindTransform(timeObject, index);
            StepForward();
            index++;
        }
    }

    private void RecordPositions(GameObject timeObject, int index)
    {
        if (stepBack) return;
        Debug.Log("Recording Positions for - " + timeObject.name);
        Vector3 oldPosition = timeObject.transform.position;
        positions[index].Add(new Vector2(oldPosition.x, oldPosition.y));
        Debug.Log(positions[index]);
    }

    private void RewindTransform(GameObject timeObject, int index)
    {
        if (!stepBack) return;
        if (positions[index].Count <= 1) return;
        controlMusic.ChangeSpeed(-1);
        OnTimeUpdate();
        Debug.Log("Rewinding - " + timeObject.name);
        timeObject.transform.position = positions[index][^1]; // [^1] = [positions[timeObjectIndex].Count - 1];
        positions[index].RemoveAt(positions[index].Count - 1);
        //for (int i = 0; i < positions[index].Count; i++)
        //{
        //        Debug.Log(index + " - " + positions[index].Count);
        //}

    }


    private void PauseResumeGame()
    {

        if (PauseGameConditions() && !isPaused)
        {
            PauseGame();
        }
        else if (!PauseGameConditions() && isPaused)
            ResumeGame();
    }

    private void StepForward()
    {
        if (!stepForward) return;

        
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        controlMusic.ChangeSpeed(0);
        Debug.Log("Paused");

    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("Resumed");
    }

    private bool PauseGameConditions()
    {
        if (movement.anyInput)
        {
            controlMusic.ChangeSpeed(1f);
            return false;
        }

        if (!isPlayerStanding() ||  stepBack)
            return false;
  
        return true;
    }
    private bool isPlayerStanding()
    {
        magnitudee = rb.velocity.magnitude;
        if (rb.velocity.magnitude < standingThreshold)
            return true;
        else
            return false;
    }

    private void OnTimeUpdate()
    {
        foreach (TimeControlled timeObject in timeObjects)
        {
            timeObject.TimeUpdate();
        }
    }

}
