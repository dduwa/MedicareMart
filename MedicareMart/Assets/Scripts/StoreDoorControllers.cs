using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorControllers : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource; // AudioSource component to play sounds
    public AudioClip doorSound; // AudioClip for door

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Ensure there's an AudioSource component

        if (animator == null) Debug.LogError("Animator not found on the door!");
        if (audioSource == null) Debug.LogError("AudioSource not found on the door!");
    }

    public void ToggleDoor()
    {
        bool isOpen = animator.GetBool("IsOpen");
        animator.SetBool("IsOpen", !isOpen);
        PlaySound();
    }

    void PlaySound()
    {
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
    }

}