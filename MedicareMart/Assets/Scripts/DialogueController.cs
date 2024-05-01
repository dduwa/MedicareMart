using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text continuePrompt; // Add a UI Text element for the continue prompt
    private string[] dialogueLines;
    private int currentLine = 0;

    void Awake()
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

        dialoguePanel.SetActive(false);
        continuePrompt.gameObject.SetActive(false); // Initially hide the continue prompt
    }

    public void ShowDialogue(string[] lines)
    {
        GameManager.Instance.ToggleCrosshair(false);  // Disable crosshair when dialogue starts
        dialogueLines = lines;
        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
        dialoguePanel.SetActive(true);
        continuePrompt.gameObject.SetActive(true); // Show prompt when dialogue starts
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialoguePanel.activeSelf)
        {
            AdvanceDialogue();
        }
    }

    private void AdvanceDialogue()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            HideDialogue();
        }
    }

    private void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        continuePrompt.gameObject.SetActive(false); // Hide the prompt when dialogue ends
        GameManager.Instance.ToggleCrosshair(true);  // Enable crosshair when dialogue ends
    }
}
