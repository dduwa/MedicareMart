using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    public enum InteractionType {Talk, OpenDoor }
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

        // Optionally disable collider or other components that trigger interaction
        //Collider collider = GetComponent<Collider>();
        //if (collider != null)
            //collider.enabled = state;
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

