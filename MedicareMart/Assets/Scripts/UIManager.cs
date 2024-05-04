using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject crosshair;
    public ObjectivesController objectivesManager;
    public GameObject pauseMenuUI;

    private void Awake()
    {
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
    void Start()
    {
        // Disable the Pause Menu UI GameObject which should be assigned in the inspector.
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    public void ToggleCrosshair(bool state)
    {
        if (crosshair != null)
            crosshair.SetActive(state);
    }

    public void ToggleCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TriggerObjective(string objective)
    {
        if (objectivesManager != null)
        {
            Debug.Log("Triggering Objective: " + objective);
            objectivesManager.AddObjective(objective);
        }
        else
        {
            Debug.Log("ObjectivesManager not initialized");
        }
    }

    public void ShowPauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            ToggleCrosshair(false);
            ToggleCursorVisibility(true);
        }
        else
        {
            Debug.LogError("Pause menu UI is not assigned in the UIManager.");
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            ToggleCrosshair(true);
            ToggleCursorVisibility(false);
        }
        else
        {
            Debug.LogError("Pause menu UI is not assigned in the UIManager.");
        }
    }
   
}

