using UnityEngine;

public class TimeTrack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform endMarker;

    [SerializeField] private float levelDistance;
    [SerializeField] private float currentDistance;

    [SerializeField] private MusicController musicController;

    private float lastPlayerPosX;
    private float playerPosX;
    float startX;

    public float playerSpeed;
    public float avarageSpeed;
    public float levelTime;
    public float progress;

    [Header("Stopwatch")]

    public float stopTime = 0f;
    public float runningTime;
    public float wholeTime;
    public bool isStopwatchRunning = true;

    void Start()
    {
        startX = player.position.x;
        float endX = endMarker.position.x;

        levelDistance = endX - startX;

        playerPosX = player.position.x;
        lastPlayerPosX = player.position.x;
    }

    private void Update()
    {
        Stopwatch();
    }

    private void FixedUpdate()
    {
        TrackProgress();
        ChangeMusicSpeed();
    }

    private void TrackProgress()
    {
        float speedDistance;

        playerPosX = player.position.x;


        currentDistance = playerPosX - startX;
        progress = currentDistance / levelDistance;

        speedDistance = playerPosX - lastPlayerPosX;
        playerSpeed = speedDistance / Time.deltaTime;
        lastPlayerPosX = playerPosX;

        avarageSpeed = currentDistance / runningTime;

        levelTime = levelDistance / playerSpeed;
    }

    private void ChangeMusicSpeed()
    {
        float musicSpeed;
        float trackDuration = musicController.SongGuration();

        //musicSpeed = trackDuration / levelTime;
        musicSpeed = Mathf.RoundToInt(playerSpeed / 12);

        if (musicSpeed == 0)
        {
            musicController.ChangeSpeed(0.75f);
            return;
        }

        musicController.ChangeSpeed(musicSpeed);
    }

    private void Stopwatch()
    {
        wholeTime = Time.time;

        if (isStopwatchRunning)
        {
            runningTime = wholeTime - stopTime;
        }

        if (!isStopwatchRunning)
        {
            stopTime = wholeTime - runningTime;
        }
    }
   
    //public void SetUpStopwatch()
    //{
    //    float time = Time.time;

    //    if (isStopwatchRunning)
    //    {
    //        runningTime = time - startTime;
    //    }
    //    else
    //    {
    //        startTime = time - runningTime;
    //    }
    //}

    
}