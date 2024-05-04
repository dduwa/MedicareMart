using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ManagerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private bool readyToWalk = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void StartTalking()
    {
        animator.SetBool("IsTalking", true);
        readyToWalk = false;
        navMeshAgent.isStopped = true;
    }

    public void StopTalking()
    {
        animator.SetBool("IsTalking", false);
        StartCoroutine(DelayStandUp());
    }

    IEnumerator DelayStandUp()
    {
        yield return new WaitForSeconds(3); // Wait before starting to stand up
    }

    // This method is called via an Animation Event at the end of the stand-up animation
    public void StandUp()
    {
        animator.SetTrigger("StandUp");
        StartCoroutine(EnableMovementAfterAnimation());
    }

    IEnumerator EnableMovementAfterAnimation()
    {
        yield return new WaitForSeconds(3);  // Adjust this duration to the length of your stand-up animation
        readyToWalk = true;
        navMeshAgent.isStopped = false;
    }



private void Update()
    {
        if (readyToWalk && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                if (currentWaypointIndex < waypoints.Length - 1)
                {
                    currentWaypointIndex++;
                    MoveToNextWaypoint();
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                    StandUp();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        if (!readyToWalk) return;

        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        animator.SetBool("IsWalking", true);
        navMeshAgent.isStopped = false;
    }
}
