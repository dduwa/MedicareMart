using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;



public class PauseMenuController : MonoBehaviour
{
    private bool isPaused = false;

    public GameObject pauseMenuUI;

    [SerializeField] public FirstPersonController firstPersonController;

    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Disable the Pause Menu UI GameObject which should be assigned in the inspector.
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Listen for the Escape key to pause the game, but not to unpause it.
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Debug.Log("Esc key pressed. Game is now paused.");
            PauseGame();
        }
    }

    public void PauseGame()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        isPaused = true;
        firstPersonController.isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
                

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ToggleCrosshair(false);
            GameManager.Instance.ToggleCursorVisibility(true);
        }
   
    }

    public void ResumeGame()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        isPaused = false;
        firstPersonController.isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ToggleCrosshair(true);
            GameManager.Instance.ToggleCursorVisibility(false);
        }

    }

   

    public void ButtonHandlerMainMenu()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick); // Play button click sound
        }
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Menu");

    }

}
