using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorControllers : Interactable // Inherit from Interactable
{
    public Animator animator;
    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
    }

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on the door!");
        }
    }

    // Implement the Interact method from the Interactable class
    public override void Interact()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        bool isOpen = animator.GetBool("IsOpen");
        animator.SetBool("IsOpen", !isOpen);
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.doorIn); // Ensure AudioManager is not null
        }
    }
}
