using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDoorController : MonoBehaviour
{
    Animator animator;
  

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        UIManager.Instance.TriggerObjective("Pick a seat and sit down.");
        animator.SetBool("isOpen", false);
   
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", true);
 
    }
}