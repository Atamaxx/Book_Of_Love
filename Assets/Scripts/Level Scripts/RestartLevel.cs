using UnityEngine;

public class RestartLevel : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool stepBack = Input.GetButton("Rewind");
        bool pause = Input.GetKeyDown(KeyCode.F);
        bool stepForward = Input.GetKeyDown(KeyCode.E);

        //if (!stepBack && !pause && !stepForward) 
        //    Time.timeScale = 0;
        //else 
        //    Time.timeScale = 1;
    }
}