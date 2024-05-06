using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public UIManager uIManager;
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
       AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

    }
    public void OnMainMenuButtonPressed()
    {
        PlayPauseSound();
        uIManager.HidePauseMenu();
        uIManager.ShowConfirmationDialog(
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
