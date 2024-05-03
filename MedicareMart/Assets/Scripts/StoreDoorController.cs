using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorController : MonoBehaviour
{
 public Animator doorAnimator;
    private bool isOpen = false;

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
        doorAnimator.SetBool("isOpen", false); // Set the initial state of the door to closed
    }

  private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger Entered by: " + other.gameObject.name); // This will log the name of the object that enters the trigger

    if (!isOpen && (other.CompareTag("Player") || other.CompareTag("NPC")))
    {
        doorAnimator.Play("DoorOpen");
        audioManager.PlaySFX(audioManager.storeDoorbell);
        isOpen = true;
    }
}


    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to a player or an NPC
        if (isOpen && (other.CompareTag("Player") || other.CompareTag("NPC")))
        {
            Invoke("CloseDoor", 2f); // Wait for 2 seconds before closing
        }
    }


    void CloseDoor()
    {
        doorAnimator.Play("DoorClose");
        isOpen = false;
    }
}
