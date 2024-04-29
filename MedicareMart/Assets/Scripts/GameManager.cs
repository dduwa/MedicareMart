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
            objectivesManager.AddObjective(objective);
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
