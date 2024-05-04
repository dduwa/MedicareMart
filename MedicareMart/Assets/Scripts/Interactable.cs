using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    public enum InteractionType {Talk, OpenDoor, ClockIn}
    public InteractionType interactionType;
    public string[] dialogueLines; // For Talk type
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
                DialogueController.Instance.ShowDialogue(dialogueLines, this.GetComponent<Interactable>());
                break;
            case InteractionType.OpenDoor:
                // Handle door interaction
                var doorController = GetComponent<StoreDoorControllers>();
                if (doorController != null)
                {
                    // Assuming you want to toggle the door (open if closed, close if open)
                    doorController.ToggleDoor(!doorController.isOpen);
                }
                else
                {
                    Debug.LogError("DoorController component is missing on this object!");
                }
                break;
            case InteractionType.ClockIn:
                // Trigger gameplay for serving customers
                DialogueController.Instance.ShowDialogue(dialogueLines, this.GetComponent<Interactable>());
                StartServingCustomers();
                break;
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

    void StartServingCustomers()
    {
        // Implement gameplay logic here to start serving customers
        Debug.Log("Gameplay started: Serving customers");
    }

}

