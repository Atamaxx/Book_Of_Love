using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName; // The name of the scene you want to load

    private void Start()
    {
        SceneManager.LoadScene(sceneName);
    }
}
