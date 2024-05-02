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
        continuePrompt.gameObject.SetActive(false);
    }

    public void ShowDialogue(string[] lines)
    {
        GameManager.Instance.ToggleCrosshair(false);
        dialogueLines = lines;
        currentLine = 0;
        dialogueText.text = dialogueLines[currentLine];
        dialoguePanel.SetActive(true);
        continuePrompt.gameObject.SetActive(true);

        // Freeze the player by disabling the FirstPersonController
        firstPersonController.enabled = false;

        if (managerController != null)
        {
            managerController.StartTalking(); // Start dialogue animations
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

        if (managerController != null)
        {
            managerController.StopTalking(); // End dialogue animations
            managerController.StandUp(); // Stand up after dialogue

            StartCoroutine(TriggerWalking());
        }
    }

    IEnumerator TriggerWalking()
    {
        // Assuming there's a delay before starting to walk
        yield return new WaitForSeconds(2); // Wait for the stand up animation to finish
        managerController.StartWalking(); // Start walking, assuming you have such a method in ManagerController
    }


}

