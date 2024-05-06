using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioManager audioManager; // Reference to the AudioManager
    private Coroutine coroutine;
    public GameObject popup; // Reference to the Popup GameObject
    public UIManager uIManager;


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

        if (uIManager != null)
        {
            uIManager.ToggleCursorVisibility(true);
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

        GameManager.Instance.StartGameFromMainMenu(); // Start the game
    }

    public void ButtonHandlerQuit()
    {
        if (audioManager != null)
        {
            // Play the button click sound.
            audioManager.PlaySFX(audioManager.buttonClick);
            // Start a coroutine to quit after the sound has finished playing.
            StartCoroutine(WaitForSoundToFinish(audioManager.buttonClick.length));
        }
        else
        {
            GameManager.Instance.QuitGame();
        }
    }

    IEnumerator WaitForSoundToFinish(float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
    }
}
