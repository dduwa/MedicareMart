using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorController : MonoBehaviour
{
 public Animator doorAnimator;
    private AudioSource audioSource; 
    private bool isOpen = false;

    void Start()
    {
        doorAnimator.SetBool("isOpen", false); // Set the initial state of the door to closed

        audioSource = GetComponent<AudioSource>();

        // If the AudioSource is not found, log an error
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject: " + gameObject.name);
        }
        else if (audioSource.clip == null)
        {
            // If the AudioSource is found but the clip is not set, log a warning
            Debug.LogWarning("AudioSource clip is not set on the GameObject: " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            doorAnimator.Play("DoorOpen");
            audioSource.Play(); // Play the door open sound
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpen)
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