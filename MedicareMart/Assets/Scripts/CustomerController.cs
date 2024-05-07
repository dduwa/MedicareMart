using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{

    public Transform[] waypoints; // Array of waypoints to follow
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;



    private bool readyToWalk = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextWaypoint();
    }


    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypointIndex].position;
        animator.SetBool("IsWalking", true);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            animator.SetBool("IsWalking", false);
            // Wait for a bit before moving to the next waypoint
            Invoke("GoToNextWaypoint", 2.0f);
        }
    }

}
