using UnityEngine;

public class TimeTrack : MonoBehaviour
{
    [SerializeField] private Transform player; // Drag the player object here in the inspector
    //public Transform startMarker; // Drag the start marker object here in the inspector
    [SerializeField] private Transform endMarker; // Drag the end marker object here in the inspector

    [SerializeField] private float levelDistance; // The total distance of the level
    [SerializeField] private float currentDistance; // The current distance the player has traveled
    public float progress; // The percentage of the level that has been passed

    Vector2 startX;

    void Start()
    {
        startX = new(player.position.x, 0);
        Vector2 endX = new(endMarker.position.x, 0);

        levelDistance = Vector2.Distance(startX, endX);
    }

    void Update()
    {
        TrackProgress(); 
    }

    private void TrackProgress()
    {
        Vector2 playerPosX = new(player.position.x, 0);


        currentDistance = Vector2.Distance(startX, playerPosX);

        progress = currentDistance / levelDistance;

    }
}