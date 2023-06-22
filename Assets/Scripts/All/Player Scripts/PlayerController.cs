using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        // Get the player's position
        Vector3 currentPosition = transform.position;

        // Update the player's position in the PlayerPositionManager
        Info.PlayerPosition = currentPosition;
    }
}