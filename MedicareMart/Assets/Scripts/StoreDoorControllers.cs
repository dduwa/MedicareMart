using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StoreDoorControllers : Interactable // Inherit from Interactable
{
    public Animator animator;
    AudioManager audioManager;
    public float doorOpenTime = 5.0f;
    private Coroutine closeDoorCoroutine;
    public bool isOpen = false;
    private int npcCount = 0;

    // Reference to the NavMesh Obstacle component
    [SerializeField] private NavMeshObstacle navMeshObstacle;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

        navMeshObstacle = GetComponent<NavMeshObstacle>();
        if (navMeshObstacle == null)
        {
            Debug.LogError("NavMeshObstacle component not found on the door!");
        }
    }

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on the door!");
        }
    }

    public override void Interact()
    {
        // Player interaction toggles the door state only if no NPCs are within the trigger zone
        if (npcCount == 0)
        {
            ToggleDoor(!isOpen);
        }
    }

    public void ToggleDoor(bool open)
    {
        isOpen = open;
        animator.SetBool("IsOpen", open);
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.doorIn);
        }

        // Enable or disable the NavMesh Obstacle based on door state
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = !open;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            npcCount++;
            if (!isOpen)
            {
                ToggleDoor(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            npcCount--;
            if (npcCount <= 0)
            {
                npcCount = 0;
                StartCoroutine(CloseDoorAfterDelay(doorOpenTime));
            }
        }
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (npcCount == 0) // Only close the door if no NPCs are inside
        {
            ToggleDoor(false);
        }
    }
}
