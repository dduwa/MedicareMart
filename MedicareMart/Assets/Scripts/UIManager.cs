using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject crosshair;
    public ObjectivesController objectivesManager;
    public GameObject pausePanel;
    public GameObject confirmPanel; // Assign in the Inspector
    public Text messageText; // Assign in the Inspector
    public Button yesButton, noButton; // Assign in the Inspector
    public Button resumeButton; // Assign in the Inspector


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // Disable the Pause Menu UI GameObject which should be assigned in the inspector.
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Disable the Confirmation Dialog GameObject which should be assigned in the inspector.
        if (confirmPanel != null)
            confirmPanel.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIComponents();
    }

    private void AssignUIComponents()
    {
        objectivesManager = FindObjectOfType<ObjectivesController>();
        // Assign UI elements; make sure to check if they exist in the scene
        crosshair = GameObject.Find("Crosshair");
        pausePanel = GameObject.Find("PausePanel");
        confirmPanel = GameObject.Find("ConfirmPanel");
        messageText = GameObject.FindWithTag("MessageText")?.GetComponent<Text>();
        yesButton = GameObject.FindWithTag("YesButton")?.GetComponent<Button>();
        noButton = GameObject.FindWithTag("NoButton")?.GetComponent<Button>();
        resumeButton = GameObject.FindWithTag("ResumeButton")?.GetComponent<Button>();

        // Check each component and log if not found
        CheckAndLogComponent(crosshair, "Crosshair");
        CheckAndLogComponent(pausePanel, "PausePanel");
        CheckAndLogComponent(confirmPanel, "ConfirmPanel");
        CheckAndLogComponent(messageText, "MessageText");
        CheckAndLogComponent(yesButton, "YesButton");
        CheckAndLogComponent(noButton, "NoButton");
        CheckAndLogComponent(resumeButton, "ResumeButton");

        // Reset UI visibility as needed
        if (pausePanel != null) pausePanel.SetActive(false);
        if (confirmPanel != null) confirmPanel.SetActive(false);
    }

    private void CheckAndLogComponent(UnityEngine.Object component, string name)
    {
        if (component == null)
            Debug.LogError($"UIManager: {name} not found in the scene.");
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
        if (pausePanel != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());


            pausePanel.SetActive(true);
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
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            ToggleCrosshair(true);
            ToggleCursorVisibility(false);
        }
        else
        {
            Debug.LogError("Pause menu UI is not assigned in the UIManager.");
        }
    }


    public void ShowConfirmationDialog(string message, Action onConfirm, Action onCancel)
    {
        Debug.Log("Showing confirmation dialog");
        messageText.text = message;

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => {
            Debug.Log("Yes button clicked");
            onConfirm?.Invoke();
            HideConfirmationDialog();
        });

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() => {
            Debug.Log("No button clicked");
            onCancel?.Invoke();
            HideConfirmationDialog();
        });

        confirmPanel.SetActive(true);
        ToggleCrosshair(false);
        ToggleCursorVisibility(true);
    }



    public void HideConfirmationDialog()
    {
        GameManager.Instance.ResumeGame();
        confirmPanel.SetActive(false);
        ToggleCrosshair(true);
        ToggleCursorVisibility(false);
    }



}

