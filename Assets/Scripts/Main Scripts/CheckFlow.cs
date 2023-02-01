using UnityEngine;

public class CheckFlow : MonoBehaviour
{
    [SerializeField] private Checkpoint[] checkpoints;
    //[SerializeField] private Transform playerTransform;

    // float playerPosX;

    void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }


    public Vector2 FindCheckpointPosition(Vector2 playerPos)
    {
        Vector2 checkpointPosition;

        Vector2 closestPos = checkpoints[0].transform.position;
        float closestDist = 0;


        int iteration = 0;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (!checkpoint.isCheckpoint) continue;

            checkpointPosition = checkpoint.transform.position;

            // if the checkpoint is to the player's right - deactivate that checkpoint
            if (checkpointPosition.x > playerPos.x)
            {
                checkpoint.DeactivateCheckpoint();
                continue;
            }

            // find closest checkpoint
            if (iteration > 0)
            {
                float dist = Vector2.Distance(playerPos, checkpointPosition);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestPos = checkpointPosition;
                    iteration++;
                }
            }
            else
            {
                closestPos = checkpointPosition;
                closestDist = Vector2.Distance(playerPos, closestPos);
                iteration++;
            }
        }



        return closestPos;
    }


    public void DeactivateCheckpoints(Vector2 playerPos)
    {
        Vector2 checkpointPosition;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (!checkpoint.isCheckpoint) continue;

            checkpointPosition = checkpoint.transform.position;

            if (checkpointPosition.x >= playerPos.x)
            {
                checkpoint.DeactivateCheckpoint();
                continue;
            }
        }
    }
}
