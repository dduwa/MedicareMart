using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false); // Ensure the pause menu is not active when the game starts
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update is being called.");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressed. Current pause state: " + isPaused);
            TogglePause();
        }
    }


    public void TogglePause()
    {
        isPaused = !isPaused;
        gameObject.SetActive(isPaused); // This will activate or deactivate the entire PauseMenu GameObject
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

