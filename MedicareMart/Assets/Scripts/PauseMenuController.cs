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
        if (GameManager.Instance.IsGamePaused) // Make sure the game is paused before going to main menu
        {
            GameManager.Instance.TogglePause(); // Unpause the game or ensure proper game state before switching
        }
        PlayPauseSound();
        SceneManager.LoadSceneAsync("Menu");
    }


}
