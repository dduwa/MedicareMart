using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorControllers : InteractiveObjectBase   // Change the class name to StoreDoorControllers
{
    Animator animator;
    bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
        }
        else
        {
            animator.SetTrigger("Open");
        }
        isOpen = !isOpen;
    }
}
