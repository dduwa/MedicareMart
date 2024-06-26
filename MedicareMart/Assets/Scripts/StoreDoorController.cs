using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorController : MonoBehaviour
{
 public Animator doorAnimator;
    private bool isOpen = false;

    void Start()
    {
        doorAnimator.SetBool("isOpen", false); // Set the initial state of the door to closed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            doorAnimator.Play("DoorOpen");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.storeDoorbell);
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
