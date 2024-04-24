using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDoorControllers : MonoBehaviour, InteractiveObjectBase   
{
    public Transform sitPosition { get; private set; }  

    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        sitPosition = transform;  
    }

    public void Interact()  
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
