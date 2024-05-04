using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public Transform targetDestination;
    private NavMeshAgent agent;
    private Animator animator;
    public string[] possibleDialogues; // Array of possible dialogues

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ChooseModelAndDialogue();
        MoveToTarget();
    }

    void ChooseModelAndDialogue()
    {
        // Randomly enable one of the child model objects
        int modelIndex = Random.Range(0, transform.childCount);
        transform.GetChild(modelIndex).gameObject.SetActive(true);

        // Select a random dialogue
        string selectedDialogue = possibleDialogues[Random.Range(0, possibleDialogues.Length)];
        Debug.Log("Selected Dialogue: " + selectedDialogue); // Implement dialogue display logic
    }

    void MoveToTarget()
    {
        if (targetDestination != null)
        {
            agent.destination = targetDestination.position;
            animator.SetBool("isWalking", true);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetBool("isWalking", false);
            // Trigger any dialogue or interaction here
        }
    }
}
