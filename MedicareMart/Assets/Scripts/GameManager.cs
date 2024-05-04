using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; } = GameState.MainMenu;
    private AudioManager audioManager; // Reference to the AudioManager

    void Awake()
    {
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
    }

    public enum GameState
    {
        MainMenu,
        GameRunning,
        GamePaused,
        GameEnded
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

    public void PauseGame()
    {
        if (CurrentGameState == GameState.GameRunning)
        {
            CurrentGameState = GameState.GamePaused;
            // Freeze game time, show pause menu
            Time.timeScale = 0;
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if (CurrentGameState == GameState.GamePaused)
        {
            CurrentGameState = GameState.GameRunning;
            // Resume game time, hide pause menu
            Time.timeScale = 1;
            Debug.Log("Game Resumed");
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
