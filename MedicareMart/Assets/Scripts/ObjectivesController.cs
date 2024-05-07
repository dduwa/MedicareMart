using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesController : MonoBehaviour
{
    public GameObject objectivePanel; // Reference to the Objective Panel GameObject
    public Text objectiveTextTemplate;
    private Dictionary<string, bool> objectives = new Dictionary<string, bool>();

    private void Start()
    {
        objectiveTextTemplate.gameObject.SetActive(false); // Start with the template disabled
    }

    public void AddObjective(string objective)
    {
        // Add objective if not already present
        if (!objectives.ContainsKey(objective))
        {
            objectives[objective] = false; // Mark as incomplete
            UpdateObjectiveUI();
        }
    }

    private void UpdateObjectiveUI()
    {
        objectiveTextTemplate.gameObject.SetActive(false); // Hide the template before update

        foreach (var objective in objectives)
        {
            objectiveTextTemplate.text = "Objective: " + objective.Key;
            if (objective.Value) // If the objective is completed
            {
                objectiveTextTemplate.color = Color.green;
            }
            else
            {
                objectiveTextTemplate.color = Color.white;
            }
            objectiveTextTemplate.gameObject.SetActive(true);
        }
    }

    public void MarkObjectiveComplete(string objective)
    {
        if (objectives.ContainsKey(objective))
        {
            objectives[objective] = true; // Mark as complete
            UpdateObjectiveUI();
        }
    }

    public void ResetObjectives()
    {
        objectives.Clear();
        objectiveTextTemplate.gameObject.SetActive(false);
    }

}
