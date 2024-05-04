using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
    private AudioManager audioManager; // Reference to the AudioManager
    [SerializeField] public FirstPersonController firstPersonController;
    public bool IsGamePaused { get; private set; }
    // private GameObject pauseMenuUI;
    void Awake()
    {
        Debug.Log("GameManager Awake() called");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensures GameManager and its children are not destroyed.
        }
        else
        {
            Destroy(gameObject);
        }

        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene");
        }

        Debug.Log("AudioManager found: " + (audioManager != null));

        }
    public enum GameState
    {
        MainMenu,
        GameRunning,
        GamePaused,
        GameEnded
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        firstPersonController = FindObjectOfType<FirstPersonController>();
        if (firstPersonController != null)
        {
            Debug.Log("FirstPersonController found in the scene");
        }
        else
        {
            Debug.Log("FirstPersonController not found in the scene");
        }
    }



    public void StartGame()
    {
        if (CurrentGameState == GameState.MainMenu)
        {
            CurrentGameState = GameState.GameRunning;
            // Load the game scene or setup the game to start
            StartCoroutine(WaitForSoundToFinsh("Bus", audioManager.playButton.length));
            Debug.Log("Game Started");
        }
    }

    IEnumerator WaitForSoundToFinsh(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void EndGame()
    {
        if (CurrentGameState == GameState.GameRunning)
        {
            CurrentGameState = GameState.GameEnded;
            // Handle game ending, such as saving scores, showing end game UI, etc.
            Debug.Log("Game Ended");
        }
    }

    public void TogglePause()
    {
        if (IsGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void FPSUnpause()
    {
        firstPersonController.isPaused = false;
    }
    private void PauseGame()
    {
        IsGamePaused = true;
        if (firstPersonController != null)
        {
            firstPersonController.isPaused = true;
        }
        UIManager.Instance.ShowPauseMenu();
        Time.timeScale = 0f;
        AudioListener.pause = true;
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        if (IsGamePaused)
        {
            IsGamePaused = false;
            if (firstPersonController != null)
            {
                firstPersonController.isPaused = false;
            }
            UIManager.Instance.HidePauseMenu();
            Time.timeScale = 1f;
            AudioListener.pause = false;
            Debug.Log("Game Resumed");
        }
    }

    public void RestartGame()
    {
        // Reset any game-specific data or states here
        if (CurrentGameState != GameState.GameRunning)
        {
            // Reset game-specific variables and states
            // For example, reset score, player health, inventory, etc.

            // Load the main game scene (assuming it's named "GameScene" or similar)
            SceneManager.LoadScene("GameScene");

            // Ensure game is running at normal speed
            Time.timeScale = 1f;
            AudioListener.pause = false;

            // Set game state to running
            CurrentGameState = GameState.GameRunning;
            Debug.Log("Game restarted");
        }
        else
        {
            Debug.Log("Game is already running");
        }
    }




    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // If running in the editor, stop playing
    #else
            Application.Quit();  // Quit the application in a build
    #endif
    }


}
