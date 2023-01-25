using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private CinemachineVirtualCamera virtualCamera;
    public Transform start;
    public Transform end;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Update()
    {
        if (virtualCamera.Follow != null) return;

        virtualCamera.Follow = target;
        //Vector3 newPos = new(lastPlayer.x, lastPlayer.y + yOffset, -10f);
        //transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);

        float time = Time.time;
        float duration = 5.0f;
        start = target;

        transform.position = Vector3.Lerp(start.transform.position, end.transform.position, time / duration);

        end = target;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera.Follow = target;
    }

}
