using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorControllers : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        if (animator == null) Debug.LogError("Animator not found on the door!");
    }

    public void ToggleDoor()
    {
        bool isOpen = animator.GetBool("IsOpen");
        animator.SetBool("IsOpen", !isOpen);
    }

}