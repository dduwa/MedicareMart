using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject crosshair;
    public ObjectivesController objectivesManager;
  

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
