using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public CinemachineConfiner confiner; // The CinemachineConfiner attached to the vcam
    public float zoomSpeed = 10f;
    public float minZoom = 1f;
    public float maxZoom = 20f;
    public float moveSpeed = 5f; // Speed of camera movement

    public Transform followTarget; // The object the camera will follow
    public Transform playerTransform; // The object the camera will follow

    private Vector3 targetPos;
    private bool moveBackToPlayer = false;

    void Update()
    {
        // Handle zooming
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float currentSize = vcam.m_Lens.OrthographicSize;
            float newSize = currentSize - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            vcam.m_Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }

        // Handle camera movement
        if (Input.GetKey(KeyCode.Space)) // Right mouse button is down
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vcam.Follow = followTarget;
            targetPos.z = followTarget.position.z; // Ensure the target position is at the same z-depth as the followTarget
            moveBackToPlayer = false;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
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
            followTarget.transform.position = Vector3.Lerp(followTarget.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    // Checks if the camera is at the edge of the bounding shape
    private bool IsCameraAtEdge()
    {
        Vector3 cameraPos = vcam.transform.position;
        Vector3 confinedPos = confiner.m_BoundingShape2D.ClosestPoint(cameraPos);

        return Vector3.Distance(cameraPos, confinedPos) < 0.1f;
    }
}
