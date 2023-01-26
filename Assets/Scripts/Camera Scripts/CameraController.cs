using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f; // The speed at which the camera moves
    [SerializeField] private float zoomSpeed = 10.0f; // The speed at which the camera zooms
    [SerializeField] private float minZoom = 10.0f; // The minimum zoom level
    [SerializeField] private float maxZoom = 100.0f; // The maximum zoom level

    [SerializeField] private CinemachineVirtualCamera virtualCamera; // reference to your virtual camera
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraMover;

    public bool canMoveCamera;

    private float horizontal;
    private float vertical;
    private float zoom;

    private void Start()
    {
        virtualCamera.Follow = playerTransform;
    }

    private void Update()
    {

        horizontal = Input.GetAxis("HorizontalCamera");
        vertical = Input.GetAxis("VerticalCamera");
        zoom = Input.GetAxis("Mouse ScrollWheel");
        SwitchToCameraMovement();
    }

    private void FixedUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (!canMoveCamera) return;
        cameraMover.transform.position += moveSpeed * Time.unscaledDeltaTime * new Vector3(horizontal, vertical, 0);
        virtualCamera.m_Lens.FieldOfView -= zoom * zoomSpeed;

        // Clamp the camera's zoom level
        virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera.m_Lens.FieldOfView, minZoom, maxZoom);
    }

    private void SwitchToCameraMovement()
    {
        if (Input.GetKey(KeyCode.K) && canMoveCamera)
        {
            canMoveCamera = false;
        }

        if (Input.GetKey(KeyCode.K))
        {
            virtualCamera.Follow = cameraMover;
            canMoveCamera = true;
        }

    }
}
