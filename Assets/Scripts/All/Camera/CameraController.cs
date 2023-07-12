using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera Vcam;
    public CinemachineConfiner Confiner; // The CinemachineConfiner attached to the Vcam
    public float ZoomSpeed = 10f;
    public float MinZoom = 1f;
    public float MaxZoom = 20f;
    public float MoveSpeed = 5f; // Speed of camera movement

    public Transform followTarget; // The object the camera will follow
    public Transform playerTransform; // The object the camera will follow

    private Vector3 targetPos;
    private bool moveBackToPlayer = false;

    void Update()
    {
        // Handle zooming
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float currentSize = Vcam.m_Lens.OrthographicSize;
            float newSize = currentSize - Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            Vcam.m_Lens.OrthographicSize = Mathf.Clamp(newSize, MinZoom, MaxZoom);
        }

        // Handle camera movement
        if (Input.GetKeyDown(KeyCode.Space) && moveBackToPlayer)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vcam.Follow = followTarget;
            targetPos.z = followTarget.position.z; // Ensure the target position is at the same z-depth as the followTarget
            moveBackToPlayer = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !moveBackToPlayer)
        {
            moveBackToPlayer = true;
        }

        if (moveBackToPlayer)
        {
            targetPos = playerTransform.position;
            targetPos.z = followTarget.position.z; // Ensure the target position is at the same z-depth as the followTarget
            if (Vector3.Distance(followTarget.position, playerTransform.position) < 0.1f)
            {
                moveBackToPlayer = false;
            }
        }
        if (!IsCameraAtEdge())
        {
            followTarget.transform.position = Vector3.Lerp(followTarget.transform.position, targetPos, MoveSpeed * Time.deltaTime);
        }
    }

    // Checks if the camera is at the edge of the bounding shape
    private bool IsCameraAtEdge()
    {
        Vector3 cameraPos = Vcam.transform.position;
        Vector3 confinedPos = Confiner.m_BoundingShape2D.ClosestPoint(cameraPos);

        return Vector3.Distance(cameraPos, confinedPos) < 0.1f;
    }
}
