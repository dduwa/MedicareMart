using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text continuePrompt;
    public FirstPersonController firstPersonController;
    public ManagerController managerController; // Reference to the ManagerController
    [SerializeField] private Camera playerCamera;

    private string[] dialogueLines;
    private int currentLine = 0;
    private float originalFov;
    private Interactable currentInteractable;

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
        continuePrompt.gameObject.SetActive(false);
    }

    public void ShowDialogue(string[] lines, Interactable interactable)
    {

        currentInteractable = interactable;  // Store the reference to the interactable NPC
        GameManager.Instance.ToggleCrosshair(false);
        dialogueLines = lines;
        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
        dialoguePanel.SetActive(true);
        continuePrompt.gameObject.SetActive(true);

        firstPersonController.enabled = false;
        originalFov = playerCamera.fieldOfView;
        playerCamera.fieldOfView = Mathf.Max(30, originalFov - 20);

        if (managerController != null)
        {
            managerController.StartTalking();
        }
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
        continuePrompt.gameObject.SetActive(false);
        GameManager.Instance.ToggleCrosshair(true);

        // Enable the FirstPersonController to unfreeze the player
        firstPersonController.enabled = true;
        // Reset the camera zoom
        playerCamera.fieldOfView = originalFov;

        if (currentInteractable != null)
        {
            currentInteractable.SetInteractable(false);  // Make the NPC non-interactable
                                                         //currentInteractable = null;  // Optionally clear the reference
        }

        if (managerController != null)
        {
            managerController.StopTalking(); // End dialogue animations
            managerController.StandUp(); // Stand up after dialogue
        }
    }


}

