using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc key pressed. Toggling game pause state.");
            GameManager.Instance.TogglePause();
            PlayPauseSound();
        }
    }

    void PlayPauseSound()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick);
        }
    }
    public void OnMainMenuButtonPressed()
    {
        PlayPauseSound();
        UIManager.Instance.HidePauseMenu();
        UIManager.Instance.ShowConfirmationDialog(
            "Are you sure you want to return to the main menu? All progress will be lost.",
            ConfirmMainMenu,
            () => Debug.Log("Main Menu transition cancelled."));
    }

    private void ConfirmMainMenu()
    {
        if (GameManager.Instance.IsGamePaused)
        {
            GameManager.Instance.TogglePause(); // Ensure the game is not paused
        }
        GameManager.Instance.RestartGame(); // Reset any game state as needed
        SceneManager.LoadSceneAsync("Menu");
    }



}
