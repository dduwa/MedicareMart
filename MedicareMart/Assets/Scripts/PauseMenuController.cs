using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool isPaused = false;

    // Declare a public GameObject variable for the Pause Menu UI.
    public GameObject pauseMenuUI;

    AudioManager audioManager;

    private void Awake()
    {
        // Find the AudioManager in the scene and get the AudioManager component
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
    }

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
            Debug.Log("Esc key pressed. Current pause state: " + isPaused);
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

    IEnumerator WaitForSoundToFinishAndLoadScene(string sceneName, float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
        // Load the scene.
        SceneManager.LoadSceneAsync(sceneName);
    }


    public void ButtonHandlerSettings()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick); // Play button click sound
        }

        StartCoroutine(WaitForSoundToFinishAndLoadScene("Settings", audioManager.buttonClick.length));
    }

    public void ButtonHandlerMainMenu()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick); // Play button click sound
        }

        StartCoroutine(WaitForSoundToFinishAndLoadScene("Menu", audioManager.buttonClick.length));
    }
}
