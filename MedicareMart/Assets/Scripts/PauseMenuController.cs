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
        isPaused = true;
        firstPersonController.isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;          

        if (GameManager.Instance != null)
            GameManager.Instance.ToggleCrosshair(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        firstPersonController.isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (GameManager.Instance != null)
            GameManager.Instance.ToggleCrosshair(true);
  
    }

   

    public void ButtonHandlerMainMenu()
    {
        //Debug.Log("Settings button pressed");
        if (audioManager != null)
        {
            //Debug.Log("AudioManager found, playing click sound");
            audioManager.PlaySFX(audioManager.buttonClick); // Play button click sound
        }
        SceneManager.LoadSceneAsync("Menu");

    }

}
