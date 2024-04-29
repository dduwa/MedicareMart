using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusDriverController : MonoBehaviour
{
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Driving()
    {
        animator.SetBool("BusStop", false);

    }

    public void IdleSitting()
    {
        animator.SetBool("BusStop", true);

    }
}
