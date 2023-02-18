using System.Collections.Generic;
using UnityEngine;

public class ControlTime : MonoBehaviour
{
    [SerializeField] private TimeControlled[] _timeControlledObjects;
    [SerializeField] private List<GameObject> _timeGameObjects;

    //private GameObject player;

    [SerializeField] private TimeTrack _timeTrack;

    private Rigidbody2D _rb;


    //bool isPaused = false;

    private void Awake()
    {
        _timeControlledObjects = FindObjectsOfType<TimeControlled>();
        _timeGameObjects = new List<GameObject>();
    }
    private void Start()
    {
        FindAllTimeObjects();
    }
    void Update()
    {

    }

    private void FixedUpdate()
    {
    }



    #region Time Operations

    private void FindAllTimeObjects()
    {
        foreach (TimeControlled timeObject in _timeControlledObjects)
        {
            _timeGameObjects.Add(timeObject.gameObject);
        }
    }








    private void OnTimeUpdate()
    {
        foreach (TimeControlled timeObject in _timeControlledObjects)
        {
            timeObject.TimeUpdate();
        }
    }

    #endregion

}


//private bool isPlayerStanding()
//{
//    magnitudee = rb.velocity.magnitude;
//    if (rb.velocity.magnitude < standingThreshold)
//        return true;
//    else
//        return false;
//}

//private void AllTimeOperations()
//{
//    int index = 0;

//    foreach (GameObject timeObject in newTimeObjects)
//    {
//        if (!isPaused)
//            RecordPositions(timeObject, index);

//        RewindTransform(timeObject, index);
//        index++;
//    }
//}

//private void RecordPositions(GameObject timeObject, int index)
//{
//    if (stepBack) return;
//    Vector3 oldPosition = timeObject.transform.position;
//    positions[index].Add(new Vector2(oldPosition.x, oldPosition.y));
//}

//private void RewindTransform(GameObject timeObject, int index)
//{
//    if (!stepBack) return;
//    if (positions[index].Count <= 1) return;
//    OnTimeUpdate();
//    timeObject.transform.position = positions[index][^1]; // [^1] = [positions[timeObjectIndex].Count - 1];
//    positions[index].RemoveAt(positions[index].Count - 1);
//}

//private void PauseGame()
//{
//    controlRb.FreezeAllObjects();

//    timeTrack.isStopwatchRunning = false;

//    isPaused = true;
//    Debug.Log("Paused");

//}

//private void ResumeGame()
//{
//    controlRb.UnFreezeAllObjects();

//    timeTrack.isStopwatchRunning = true;

//    isPaused = false;
//    Debug.Log("Resumed");
//}

//private bool PauseGameConditions()
//{
//    if (false)
//        return false;

//    return true;
//}

//private void PauseResumeGame()
//{

//    if (PauseGameConditions() && !isPaused)
//    {
//        PauseGame();
//    }
//    else if (!PauseGameConditions() && isPaused)
//        ResumeGame();
//}