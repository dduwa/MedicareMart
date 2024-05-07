using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    public enum InteractionType {Talk, OpenDoor, ClockIn}
    public InteractionType interactionType;
    public DialogueLine[] dialogueLines; // For Talk type
    public bool isInteractable = true;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        HighlightObject(true);  // Highlight when the game starts
    }

    public void SetInteractable(bool state)
    {
        isInteractable = state;  // Update the interactability state
    }

    public virtual void Interact()
    {
        if (!isInteractable) return;
        // This method is overridden by all interactable objects.
        Debug.Log("Interacted with " + gameObject.name);
        switch (interactionType)
        {
            case InteractionType.Talk:
                DialogueController.Instance.ShowDialogue(new List<DialogueLine>(dialogueLines), this.GetComponent<Interactable>());
                break;
            case InteractionType.OpenDoor:
                // Handle door interaction
                ToggleDoor();
                break;
            case InteractionType.ClockIn:
                // Trigger gameplay for serving customers
                StartCoroutine(PlayClockInCutscene());
                //DialogueController.Instance.ShowDialogue(new List<DialogueLine>(dialogueLines), this);
                break;
        }
    }
   

    void ToggleDoor()
    {
        var doorController = GetComponent<StoreDoorControllers>();
        if (doorController != null)
        {
            doorController.ToggleDoor(!doorController.isOpen);
        }
        else
        {
            Debug.LogError("DoorController component is missing on this object!");
        }
    }

    void HighlightObject(bool highlight)
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("No Renderer found on the object!");
            return;
        }

        renderer.material = highlight ? highlightMaterial : originalMaterial;
    }
    IEnumerator PlayClockInCutscene()
    {
        CutsceneController.Instance.StartCutscene(3);  
        yield return new WaitForSeconds(5);  // Wait for the length of the cutscene
        UIManager.Instance.ToggleCrosshair(true);

        StartServingCustomers();  // This method now triggers spawning
    }

    void StartServingCustomers()
    {
        Debug.Log("Gameplay started: Serving customers");
        Spawner.Instance.SpawnCustomer();  // Start spawning customers
        SetInteractable(false);  // Disable further interactions if not needed
    }


}

