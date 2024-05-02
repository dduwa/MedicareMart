using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ManagerController : MonoBehaviour
{
    Animator animator;
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool readyToWalk = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void StartTalking()
    {
        animator.SetBool("IsTalking", true);
        readyToWalk = false;
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
        animator.SetTrigger("StandUp");
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stand Up"))
        {
            // Ensures the stand-up animation completes before walking starts
            readyToWalk = true;
        }
    }

    public void Update()
    {
        Debug.DrawLine(transform.position, waypoints[currentWaypointIndex].position, Color.red);

        if (readyToWalk && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (currentWaypointIndex < waypoints.Length)
            {
                StartWalkingToNextWaypoint();
            }
            else
            {
                animator.SetBool("IsWalking", false); // Stop walking animation
            }
        }
    }

    public void StartWalkingToNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length) return;

        Vector3 targetDirection = waypoints[currentWaypointIndex].position - transform.position;
        targetDirection.y = 0; // Keep the direction strictly horizontal
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * navMeshAgent.angularSpeed);

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        animator.SetBool("IsWalking", true);
    }

}


