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
    //public float musicSpeed;
    public float levelTime;
    public float progress;



    void Start()
    {
        startX = player.position.x;
        float endX = endMarker.position.x;

        levelDistance = endX - startX;

        playerPosX = player.position.x;
        lastPlayerPosX = player.position.x;
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

}