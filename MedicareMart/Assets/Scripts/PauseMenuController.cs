using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool isPaused = false;

    // Declare a public GameObject variable for the Pause Menu UI.
    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        // Instead of disabling the gameObject this script is attached to,
        // disable the Pause Menu UI GameObject which should be assigned in the inspector.
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update is being called.");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Q key pressed. Current pause state: " + isPaused);
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Use the pauseMenuUI reference to activate or deactivate the pause menu
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f; // This will pause or resume the game

        // Set the cursor state
        Cursor.visible = isPaused; // Show cursor when paused, hide when not paused
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked; // Lock cursor when not paused
    }

    public void ButtonHandlerSettings()
    {
        // Consider pausing time or handling pause state appropriately when going to settings
        SceneManager.LoadSceneAsync("Settings");
    }

    public void ButtonHandlerMainMenu()
    {
        Time.timeScale = 1f; // Make sure the game isn't paused when returning to the main menu
        SceneManager.LoadSceneAsync("Menu");
    }
}
