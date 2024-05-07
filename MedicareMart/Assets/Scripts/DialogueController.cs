using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class DialogueController : MonoBehaviour
{
    //singleton 
    public static DialogueController Instance { get; private set; }

    public GameObject dialoguePanel;
    public Text dialogueText;
    public Button[] choiceButtons;  // Buttons for player choices
    public Text continuePrompt;


    public FirstPersonController firstPersonController;
    public ManagerController managerController; // Reference to the ManagerController
    [SerializeField] private Camera playerCamera;

    private List<DialogueLine> currentDialogue = new List<DialogueLine>();
    private int currentLine = 0;
    private float originalFov;
    private Interactable currentInteractable;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dialoguePanel.SetActive(false);
        continuePrompt.gameObject.SetActive(false);
        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false);
    }

    public void ShowDialogue(List<DialogueLine> lines, Interactable interactable)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpscare);
        currentInteractable = interactable;
        UIManager.Instance.ToggleCrosshair(false);

        currentDialogue = lines; 
        currentLine = 0;
        ShowCurrentLine();
        dialoguePanel.SetActive(true);
        firstPersonController.enabled = false;
        originalFov = playerCamera.fieldOfView;
        playerCamera.fieldOfView = Mathf.Max(30, originalFov - 20);

        if (managerController != null)
        {
            managerController.StartTalking();
        }
    }


    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue[currentLine];
        dialogueText.text = line.text;
        if (line.options != null && line.options.Length > 0)
        {
            continuePrompt.gameObject.SetActive(false);
            ShowOptions(line.options);
        }
        else
        {
            continuePrompt.gameObject.SetActive(true);
            foreach (var button in choiceButtons)
                button.gameObject.SetActive(false);
        }
    }

    private void ShowOptions(DialogueOption[] options)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < options.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<Text>().text = (i + 1) + ": " + options[i].text;
                int nextIndex = currentLine + 1;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => ChooseOption(options[i]));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChooseOption(DialogueOption option)
    {
        ScoreManager.Instance.AddScore(option.scoreImpact);
        if (option.nextLines != null && option.nextLines.Length > 0)
        {
            currentDialogue = new List<DialogueLine>(option.nextLines);
            currentLine = 0;
            ShowCurrentLine();
        }
        else
        {
            HideDialogue();
        }
    }

    private void HandleOptionSelection()
    {
        if (currentDialogue.Count > 0 && currentDialogue[currentLine].options != null)
        {
            DialogueOption[] options = currentDialogue[currentLine].options;
            for (int i = 0; i < options.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))  // '1' for the first option, '2' for the second, etc.
                {
                    ChooseOption(options[i]);
                    break;  // Break the loop after a choice is made to avoid multiple selections
                }
            }
        }
    }

    void Update()
    {
        // Check if the dialogue panel is active and if it's appropriate to advance the dialogue
        if (Input.GetKeyDown(KeyCode.E) && dialoguePanel.activeSelf && !IsOptionActive())
        {
            AdvanceDialogue();
        }
        HandleOptionSelection();
    }

    private void AdvanceDialogue()
    {
        if (currentLine + 1 < currentDialogue.Count)
        {
            currentLine++;
            ShowCurrentLine();
        }
        else
        {
            HideDialogue();
        }
    }

    // Check if any choice buttons are currently active
    private bool IsOptionActive()
    {
        foreach (var button in choiceButtons)
        {
            if (button.gameObject.activeSelf)
            {
                return true;  // Return true if any button is active
            }
        }
        return false;  // No buttons are active
    }


    private void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        continuePrompt.gameObject.SetActive(false);
        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false);

        UIManager.Instance.ToggleCrosshair(true);
        firstPersonController.enabled = true;
        playerCamera.fieldOfView = originalFov;

        if (currentInteractable != null)
        {
            currentInteractable.SetInteractable(false);  // Make the NPC non-interactable
                                                         
        }

        if (managerController != null)
        {
            managerController.StopTalking(); // End dialogue animations
            managerController.StandUp(); // Stand up after dialogue
        }
        UIManager.Instance.CompleteObjective("Speak to the manager");
        UIManager.Instance.TriggerObjective("Clock in when ready.");
        GameManager.Instance.DialogueEnded();

    }




}

