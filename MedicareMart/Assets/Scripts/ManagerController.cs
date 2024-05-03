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
        yield return new WaitForSeconds(1);
        StandUp();
    }

    public void StandUp()
    {
        animator.SetTrigger("StandUp");
        readyToWalk = true;
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
                    readyToWalk = false;
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
