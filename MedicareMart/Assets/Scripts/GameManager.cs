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
            DontDestroyOnLoad(gameObject);  
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
    }

   

    void UpdateGameStateBasedOnScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                CurrentGameState = GameState.MainMenu;
                break;
            case "Bus":
            case "Store":
                CurrentGameState = GameState.GameRunning;  // Treat both scenes as part of general gameplay
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
            //UIManager.Instance.TriggerObjective("Speak to the manager");

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
        else if(finalScore > 20)
        {
            Debug.Log("Score is greater than 50. Ending game.");
            EndGame();
        }
        else if(finalScore < 20)
        {
            Debug.Log("Score is not 0. Continuing game normally.");
            EndGame();
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
        else if(score > 20)
        {
            Debug.Log("Score is greater than 50 after dialogue. Ending game.");
            EndGame();
        }
        else if(score == 12)
        {
            EndGame();
        }else
        {
            Debug.Log("Score is not 0 after dialogue. Continuing game normally.");
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
        if (CurrentGameState == GameState.GameRunning || CurrentGameState == GameState.GamePaused || CurrentGameState == GameState.GameEnded)
        {
            // Ensure the game is not paused
            if (IsGamePaused)
            {
                ResumeGame();
            }

            ScoreManager.Instance.ResetScore();


            SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single).completed += (AsyncOperation operation) =>
            {
                // Ensure the game state is correctly set once the scene is loaded
                CurrentGameState = GameState.MainMenu;
                Time.timeScale = 1f;
                AudioListener.pause = false;

                Debug.Log("Game restarted to initial state");
            };
        }
        else
        {
            Debug.Log("Attempt to restart game denied: Game is in a state that does not allow restarting.");
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
