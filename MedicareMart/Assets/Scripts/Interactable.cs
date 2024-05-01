using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    public enum InteractionType { ReadNote, Talk, OpenDoor }
    public InteractionType interactionType;
    public string[] dialogueLines; // For ReadNote and Talk types

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        HighlightObject(true);  // Highlight when the game starts
    }

    public virtual void Interact()
    {
        // This method is overridden by all interactable objects.
        Debug.Log("Interacted with " + gameObject.name);
        switch (interactionType)
        {
            case InteractionType.ReadNote:
                DialogueController.Instance.ShowDialogue(dialogueLines);
                break;
            case InteractionType.Talk:
                DialogueController.Instance.ShowDialogue(dialogueLines);
                break;
            case InteractionType.OpenDoor:
                // Handle door interaction
                var doorController = GetComponent<StoreDoorControllers>();
                if (doorController != null)
                {
                    doorController.ToggleDoor();
                }
                else
                {
                    Debug.LogError("DoorController component is missing on this object!");
                }
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

}

