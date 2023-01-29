using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f; // The speed at which the camera moves
    [SerializeField] private float zoomSpeed = 10.0f; // The speed at which the camera zooms
    [SerializeField] private float minZoom = 10.0f; // The minimum zoom level
    [SerializeField] private float maxZoom = 100.0f; // The maximum zoom level

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera secondCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraMover;

    public bool movingCamera;

    private float horizontal;
    private float vertical;
    [SerializeField] private float zoom;

    private void Start()
    {
        playerCamera.Follow = playerTransform;
        secondCamera.Follow = cameraMover;
    }

    private void Update()
    {
        zoom = Input.GetAxis("Mouse ScrollWheel");
        SwitchToCameraMovement();
    }

    private void FixedUpdate()
    {
        //if (!movingCamera) return;

        //ZoomSecondCamera();

    }

    private void ZoomSecondCamera()
    {
        if (zoom > 0.05f || zoom < -0.05)
        {
            secondCamera.m_Lens.OrthographicSize += zoom * zoomSpeed;
            // Clamp the camera's zoom level
            secondCamera.m_Lens.OrthographicSize = Mathf.Clamp(secondCamera.m_Lens.FieldOfView, minZoom, maxZoom);
        }

    }

    private void SwitchToCameraMovement()
    {
        bool switchKey = Input.GetKeyDown(KeyCode.K);
        if (switchKey && !movingCamera)
        {
            secondCamera.Priority = 1;
            playerCamera.Priority = 0;
            movingCamera = true;
        }
        else if (switchKey && movingCamera)
        {
            secondCamera.Priority = 0;
            playerCamera.Priority = 1;
            movingCamera = false;
        }

    }
}
