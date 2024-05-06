using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private Coroutine coroutine;
    public GameObject popup; // Reference to the Popup GameObject

    private void Awake()
    {

        // Initialize Popup as not active when the scene starts
        if (popup != null)
        {
            popup.SetActive(false);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ToggleCursorVisibility(true);
        }
    }

    // Add a new function to handle the opening of the popup
    public void OpenPopup()
    {
        if (popup != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            popup.SetActive(true);
        }
    }

    // Add a new function to handle the closing of the popup
    public void ClosePopup()
    {
        if (popup != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            popup.SetActive(false);
        }
    }

    public void ButtonHandlerPlay()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.playButton); // Play button sound
       
        GameManager.Instance.StartGameFromMainMenu(); // Start the game
    }

    public void ButtonHandlerQuit()
    {
        // Play the button click sound.
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        GameManager.Instance.QuitGame();
       
    }
}
