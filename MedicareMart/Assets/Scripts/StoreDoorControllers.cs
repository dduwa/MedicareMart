using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorControllers : Interactable // Inherit from Interactable
{
    public Animator animator;
    AudioManager audioManager;

    public float doorOpenTime = 5.0f;  // Time in seconds the door remains open

    private Coroutine closeDoorCoroutine;
    private int npcCount = 0; // Track the number of NPCs within the trigger

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
        bool isOpen = animator.GetBool("IsOpen");
        ToggleDoor(!isOpen);
    }

    public void ToggleDoor(bool open)
    {
        if (animator.GetBool("IsOpen") != open)
        {
            animator.SetBool("IsOpen", open);
            if (audioManager != null)
            {
                audioManager.PlaySFX(open ? audioManager.doorIn : audioManager.doorIn);
            }
            if (open)
            {
                // Start or restart the coroutine to close the door after a delay
                if (closeDoorCoroutine != null)
                {
                    StopCoroutine(closeDoorCoroutine);
                }
                closeDoorCoroutine = StartCoroutine(CloseDoorAfterDelay(doorOpenTime));
            }
        }
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (npcCount == 0) // Only close if no NPCs are inside
        {
            ToggleDoor(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) // Make sure your NPCs have this tag
        {
            npcCount++;
            ToggleDoor(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npcCount--;
            if (npcCount <= 0)
            {
                npcCount = 0; // Prevents negative count
                ToggleDoor(false);
            }
        }
    }
}
