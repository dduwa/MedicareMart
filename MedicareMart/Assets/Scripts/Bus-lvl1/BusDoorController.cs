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
        GameManager.Instance.TriggerObjective("Speak to the Manager.");
        animator.SetBool("isOpen", false);
   
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", true);
 
    }
}