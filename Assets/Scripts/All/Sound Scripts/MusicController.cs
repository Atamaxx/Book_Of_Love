using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    [Range(0, 10)] [SerializeField] private float speed = 1;

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

    public float SongGuration()
    {
        return musicSource.clip.length;
    }

    public void Reverse()
    {
        speed = -1;
    }
}