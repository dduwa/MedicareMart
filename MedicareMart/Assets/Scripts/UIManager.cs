using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{

    public GameObject crosshair;
    public ObjectivesController objectivesManager;
    public GameObject pauseMenuUI;
    public GameObject confirmationDialog; // Assign in the Inspector
    public Text messageText; // Assign in the Inspector
    public Button yesButton, noButton; // Assign in the Inspector
    public Button resumeButton; // Assign in the Inspector

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Implement the re-initialization or checking of necessary components here
       RefreshUIElements();
    }

    private void RefreshUIElements()
    {
        // Find and assign the Crosshair UI element by its tag or name.
        if (crosshair == null)
        {
            crosshair = GameObject.FindWithTag("Crosshair");
            if (crosshair == null)
            {
                Debug.LogWarning("Crosshair canvas not found in the current scene.");
            }
        }

        // Find and assign the Objectives Manager by searching for its component.
        if (objectivesManager == null)
        {
            objectivesManager = FindObjectOfType<ObjectivesController>();
            if (objectivesManager == null)
            {
                Debug.LogWarning("Objectives Manager component not found in the current scene.");
            }
        }

        // Find and assign the Pause Menu UI element by its tag or name.
        if (pauseMenuUI == null)
        {
            pauseMenuUI = GameObject.FindWithTag("PausePanel");
            if (pauseMenuUI != null)
            {
                pauseMenuUI.SetActive(false); // Ensure the pause menu is initially hidden.
            }
            else
            {
                Debug.LogWarning("Pause menu panel not found in the current scene.");
            }
        }

        // Find and assign the Confirmation Dialog UI element by its tag or name.
        if (confirmationDialog == null)
        {
            confirmationDialog = GameObject.FindWithTag("ConfirmPanel");
            if (confirmationDialog != null)
            {
                confirmationDialog.SetActive(false); // Ensure the confirmation dialog is initially hidden.
            }
            else
            {
                Debug.LogWarning("Confirmation dialog panel not found in the current scene.");
            }
        }

        // Find and rebind the Message Text within the Confirmation Dialog.
        if (messageText == null)
        {
            GameObject messageTextObj = GameObject.FindWithTag("MessageText");
            if (messageTextObj != null)
            {
                messageText = messageTextObj.GetComponent<Text>();
            }
            else
            {
                Debug.LogWarning("Message text not found in the current scene.");
            }
        }

        // Find and rebind the Yes Button and its listener.
        if (yesButton == null)
        {
            GameObject yesButtonObj = GameObject.FindWithTag("YesButton");
            if (yesButtonObj != null)
            {
                yesButton = yesButtonObj.GetComponent<Button>();
                yesButton.onClick.RemoveAllListeners();
                yesButton.onClick.AddListener(() => {
                    Debug.Log("Yes button clicked");
                    // Add any specific methods you want to call when Yes is clicked
                });
            }
            else
            {
                Debug.LogWarning("Yes button not found in the current scene.");
            }
        }

        // Find and rebind the No Button and its listener.
        if (noButton == null)
        {
            GameObject noButtonObj = GameObject.FindWithTag("NoButton");
            if (noButtonObj != null)
            {
                noButton = noButtonObj.GetComponent<Button>();
                noButton.onClick.RemoveAllListeners();
                noButton.onClick.AddListener(() => {
                    Debug.Log("No button clicked");
                    // Add any specific methods you want to call when No is clicked
                });
            }
            else
            {
                Debug.LogWarning("No button not found in the current scene.");
            }
        }

        // Find and rebind the Resume Button and its listener.
        if (resumeButton == null)
        {
            GameObject resumeButtonObj = GameObject.FindWithTag("ResumeButton");
            if (resumeButtonObj != null)
            {
                resumeButton = resumeButtonObj.GetComponent<Button>();
                resumeButton.onClick.RemoveAllListeners();
                resumeButton.onClick.AddListener(() => {
                    Debug.Log("Resume button clicked");
                    GameManager.Instance.ResumeGame();
                });
            }
            else
            {
                Debug.LogWarning("Resume button not found in the current scene.");
            }
        }

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            ToggleCursorVisibility(true);
            pauseMenuUI.SetActive(false);
        }
        else
        {
            ToggleCursorVisibility(false);
            pauseMenuUI.SetActive(true);
        }

    }



    void Start()
    {
        // Disable the Pause Menu UI GameObject which should be assigned in the inspector.
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Disable the Confirmation Dialog GameObject which should be assigned in the inspector.
        if (confirmationDialog != null)
            confirmationDialog.SetActive(false);
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
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());


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

        confirmationDialog.SetActive(true);
        ToggleCrosshair(false);
        ToggleCursorVisibility(true);
    }



    public void HideConfirmationDialog()
    {
        GameManager.Instance.ResumeGame();
        confirmationDialog.SetActive(false);
        ToggleCrosshair(true);
        ToggleCursorVisibility(false);
    }



}

