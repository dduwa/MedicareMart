using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject crosshair;
    public ObjectivesController objectivesManager;


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

        InitializeComponents(); // Ensure components are initialized properly at start
    }
    void InitializeComponents()
    {
        // Set up or find other necessary components
        FindCrosshair();  // Find and set the crosshair
        FindObjectiveManager();  // Ensure the objectives manager is found
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe to clean up
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCrosshair();  // Re-find and assign the crosshair every time a scene is loaded
        FindObjectiveManager();  // Also ensure the objectives manager is found
    }

    void FindCrosshair()
    {
        GameObject crosshairObject = GameObject.FindGameObjectWithTag("Crosshair");
        if (crosshairObject != null)
        {
            crosshair = crosshairObject;
            DontDestroyOnLoad(crosshair);  // Make sure crosshair is persistent across scenes
        }
        else
        {
            Debug.LogError("Crosshair object not found with tag 'Crosshair'");
        }
    }


    private void FindObjectiveManager()
    {
        objectivesManager = FindObjectOfType<ObjectivesController>();  // Find the ObjectivesManager in the current scene
        if (objectivesManager == null)
        {
            Debug.LogError("ObjectivesManager not found!");  // Log an error if not found
        }
        else
        {
            ResetState();  // Reset the state if the ObjectivesManager is found
        }
    }

    public void ToggleCrosshair(bool state)
    {
        if(crosshair != null)
            crosshair.SetActive(state);
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


    public void ToggleCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ResetState()
    {
        ToggleCrosshair(true);  // Example: Always turn on crosshair when resetting state
        ToggleCursorVisibility(true);  // Set cursor visibility as needed

        if (objectivesManager != null)
        {
            objectivesManager.ResetObjectives();
        }
        else
        {
            Debug.LogError("ObjectivesManager not initialized");
        }
    }


}
