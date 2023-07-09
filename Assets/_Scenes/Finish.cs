using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class Finish : MonoBehaviour
{
    [Scene]
    public string sceneName; // Name of the scene you want to load

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming the player object has a "Player" tag
        {
            SceneManager.LoadScene(sceneName); // Load the specified scene
        }
    }
}