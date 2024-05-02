using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerController : MonoBehaviour
{
    Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartTalking()
    {
        animator.SetBool("IsTalking", true);
    }

    public void StopTalking()
    {
        animator.SetBool("IsTalking", false);
        StartCoroutine(DelayStandUp());
    }

    IEnumerator DelayStandUp()
    {
        yield return new WaitForSeconds(1); // Adjust time based on your need
        StandUp();
    }

    public void StandUp()
    {
        Debug.Log("StandUp method called."); // Check if this method is executed
        animator.SetTrigger("StandUp");
    }


    public void StartWalking()
    {
        animator.SetBool("IsWalking", true);
    }

}
