using UnityEngine;

public class MouseLock : MonoBehaviour
{
    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        // If the player presses Esc, unlock the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        // If the player presses any mouse button, lock the cursor
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        // Lock the cursor in the center of the screen
        Cursor.lockState = CursorLockMode.Confined;
        // Make the cursor invisible
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        // Unlock the cursor so it can move freely
        Cursor.lockState = CursorLockMode.None;
        // Make the cursor visible
        Cursor.visible = true;
    }
}
