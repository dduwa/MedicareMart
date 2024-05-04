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
        OnBus,        // When the player is on the bus
        InStore,      // When the player is in the store
        GameRunning,  // General game running state for other gameplay
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
            Debug.Log("FirstPersonController found in the scene: " + scene.name);
            firstPersonController.isPaused = (CurrentGameState == GameState.GamePaused); // Ensure paused state is consistent
        }
        else
        {
            Debug.Log("FirstPersonController not found in the scene: " + scene.name);
        }
        UpdateGameStateBasedOnScene(scene.name); // Ensure the game state is updated according to the loaded scene
    }

    public void StartGameFromMainMenu()
    {
        if (CurrentGameState == GameState.MainMenu)
        {
            CurrentGameState = GameState.OnBus;
            StartCoroutine(WaitForSoundToFinish("Bus", audioManager.playButton.length)); // Transition to the bus
            Debug.Log("Transitioning from Main Menu to Bus");
        }
    }




    public void StartGameFromBus()
    {
        if (CurrentGameState == GameState.OnBus)
        {
            CurrentGameState = GameState.InStore;
            StartCoroutine(WaitForSoundToFinish("Store", audioManager.playButton.length)); // Transition to the store
            Debug.Log("Transitioning from Bus to Store");
        }
    }

    IEnumerator WaitForSoundToFinish(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
        UpdateGameStateBasedOnScene(sceneName);
    }

    private void UpdateGameStateBasedOnScene(string sceneName)
    {
        switch (sceneName)
        {
            case "BusScene":
                CurrentGameState = GameState.OnBus;
                break;
            case "StoreScene":
                CurrentGameState = GameState.InStore;
                break;
            case "MainGameScene":
                CurrentGameState = GameState.GameRunning;
                break;
            default:
                Debug.LogError("Unknown scene");
                break;
        }
        Debug.Log("Loaded scene and updated game state: " + sceneName);
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
        Debug.Log("Game is quitting...");  // This log will confirm the function is called
    }



}
