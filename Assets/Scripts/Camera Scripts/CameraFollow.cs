using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        if (virtualCamera.Follow != null) return;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera.Follow = target;
        //Vector3 newPos = new(lastPlayer.x, lastPlayer.y + yOffset, -10f);
        //transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

}
