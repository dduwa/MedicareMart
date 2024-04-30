using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioManager audioManager; // Reference to the AudioManager
    private Coroutine coroutine;
    public GameObject popup; // Reference to the Popup GameObject

    private void Awake()
    {
        // Find the AudioManager object in the scene and assign it to the audioManager variable.
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

        // Initialize Popup as not active when the scene starts
        if (popup != null)
        {
            popup.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ToggleCursorVisibility(true);
        }
    }

    // Add a new function to handle the opening of the popup
    public void OpenPopup()
    {
        if (popup != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            popup.SetActive(true);
        }
    }

    // Add a new function to handle the closing of the popup
    public void ClosePopup()
    {
        if (popup != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick);
            popup.SetActive(false);
        }
    }

    public void ButtonHandlerPlay()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.playButton); // Play button sound
        }

        StartCoroutine(WaitForSoundToFinishAndLoadScene("Bus", audioManager.playButton.length));
    }


    IEnumerator WaitForSoundToFinishAndLoadScene(string sceneName, float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
        // Load the scene.
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ButtonHandlerQuit()
    {
        if (audioManager != null)
        {
            // Play the button click sound.
            audioManager.PlaySFX(audioManager.buttonClick);

            // Start a coroutine to quit after the sound has finished playing.
            StartCoroutine(WaitForSoundToFinishAndQuit(audioManager.buttonClick.length));
        }
        else
        {
            // If AudioManager is not found, quit immediately.
            QuitApplication();
        }
    }

    IEnumerator WaitForSoundToFinishAndQuit(float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
        // Quit the application.
        QuitApplication();
    }

    private void QuitApplication()
    {
        // If we are running in a standalone build of the game
#if UNITY_STANDALONE
    // Quit the application
    Application.Quit();
#endif

        // If we are running in the editor
#if UNITY_EDITOR
    // Stop playing the scene
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
