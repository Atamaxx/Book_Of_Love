using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource; // Drag the music source here in the inspector
    [Range(0, 10)] [SerializeField] private float speed = 1; // The speed that the music should play at

    private void Start()
    {
       musicSource.Play();
    }
    void Update()
    {
        musicSource.pitch = speed;
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Reverse()
    {
        speed = -1;
    }
}