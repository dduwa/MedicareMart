using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
    [SerializeField] public FirstPersonController firstPersonController;
    public bool IsGamePaused { get; private set; }

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

    public void SetGameState(GameState newState)
    {
        Debug.Log($"Game state changing from {CurrentGameState} to {newState}");
        CurrentGameState = newState;
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
        UpdateGameStateBasedOnScene(scene.name); 

        if (scene.name == "Endings") 
        {
           
            CutsceneController.Instance.StartCutscene(3);
        }
    }

    void UpdateGameStateBasedOnScene(string sceneName)
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
            case "Endings":
                CurrentGameState = GameState.GameEnded;
                break;
            default:
                Debug.LogError($"Unknown scene: {sceneName}");
                break;
        }
        Debug.Log($"Loaded scene {sceneName}, updated game state to {CurrentGameState}");
    }


    public void StartGameFromMainMenu()
    {
        if (CurrentGameState == GameState.MainMenu)
        {
            CurrentGameState = GameState.OnBus;
            StartCoroutine(WaitForSoundToFinish("Bus", AudioManager.Instance.playButton.length)); // Transition to the bus
            Debug.Log("Transitioning from Main Menu to Bus");
        }
    }


    public void StartGameFromBus()
    {
        if (CurrentGameState == GameState.OnBus)
        {
            CurrentGameState = GameState.InStore;
            StartCoroutine(WaitForSoundToFinish("Store", AudioManager.Instance.playButton.length)); // Transition to the store
            Debug.Log("Transitioning from Bus to Store");
        }
    }

    IEnumerator WaitForSoundToFinish(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
        UpdateGameStateBasedOnScene(sceneName);
    }

    public void EvaluateEndings()
    {
        int finalScore = ScoreManager.Instance.GetScore();
        Debug.Log($"Final Score: {finalScore}, evaluating endings...");
        if (finalScore == 0)
        {
            Debug.Log("Score is 0. Ending game.");
            EndGame();
        }
        else
        {
            Debug.Log("Score is not 0. Continuing game normally.");
        }   
    }

    public void DialogueEnded()
    {
        int score = ScoreManager.Instance.GetScore();
        if (score == 0)
        {
            Debug.Log("Score is 0 after dialogue. Ending game.");
            EndGame();
        }
        else
        {
            Debug.Log("Dialogue ended with score " + score + ". Continuing game normally.");
        }
    }

    public void EndGame()
    {
        Debug.Log($"Attempting to end game from state: {CurrentGameState}");
        if (CurrentGameState == GameState.GameRunning)
        {
            CurrentGameState = GameState.GameEnded;
            Debug.Log("Game Ended, loading endings scene.");
            SceneManager.LoadSceneAsync("Endings");  // Load the ending scene
        }
        else
        {
            Debug.Log("EndGame called, but game state is not running.");
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

            foreach (var source in FindObjectsOfType<AudioSource>())
            {
                source.Stop();
            }
            ScoreManager.Instance.ResetScore();

            CurrentGameState = GameState.MainMenu;

            SceneManager.LoadSceneAsync("Menu");

            // Ensure game is running at normal speed
            Time.timeScale = 1f;
            AudioListener.pause = false;

            Debug.Log("Game restarted to initial state");

        }
        else
        {
            Debug.Log("Game is already running");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game is preparing to quit...");
        StartCoroutine(WaitForQuitSound(AudioManager.Instance.buttonClick.length));
    }

    IEnumerator WaitForQuitSound(float delay)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        yield return new WaitForSeconds(delay);

        Debug.Log("Game is quitting...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // If running in the editor, stop playing
#else
        Application.Quit();  // Quit the application in a build
#endif
    }


}
