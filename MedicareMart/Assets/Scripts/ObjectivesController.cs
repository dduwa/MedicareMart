using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesController : MonoBehaviour
{
    public GameObject objectivePanel; // Reference to the Objective Panel GameObject
    public Text objectiveTextTemplate;
    public List<string> objectives = new List<string>();

    // Start is called before the first frame update
    private void Start()
    {
        objectiveTextTemplate.gameObject.SetActive(false); // Start with the template disabled
    }

    public void AddObjective(string objective)
    {
        // Clear any existing objective
        ClearObjectives();

        // Update the text of the template and activate it
        objectiveTextTemplate.text = "Objective: " + objective;
        objectiveTextTemplate.gameObject.SetActive(true);
    }

    private void ClearObjectives()
    {
        // Deactivate the current objective text
        objectiveTextTemplate.gameObject.SetActive(false);
    }

    // reset the objectives list
    public void ResetObjectives()
    {
        objectives.Clear();
    }

    public void disableObjectivePanel()
    {
        objectivePanel.SetActive(false);
    }


}
