using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class StoreDoorControllers : Interactable
{
    public Animator animator;
    AudioManager audioManager;

    public float doorOpenTime = 5.0f;
    private Coroutine closeDoorCoroutine;
    public bool isOpen = false;
    private int npcCount = 0;

    // Reference to the NavMesh Obstacle component
    [SerializeField]  private  NavMeshObstacle navMeshObstacle;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

        // Get the NavMesh Obstacle component
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
        ToggleDoor(!isOpen);
    }

    public void ToggleDoor(bool open)
    {
        if (isOpen != open)
        {
            isOpen = open;
            animator.SetBool("IsOpen", open);
            if (audioManager != null)
            {
                audioManager.PlaySFX(open ? audioManager.doorIn : audioManager.doorOut);
            }
            if (open)
            {
                if (closeDoorCoroutine != null)
                {
                    StopCoroutine(closeDoorCoroutine);
                }
                closeDoorCoroutine = StartCoroutine(CloseDoorAfterDelay(doorOpenTime));
            }
        }

        // Enable or disable the NavMesh Obstacle based on door state
        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = !open;
        }
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (npcCount == 0)
        {
            ToggleDoor(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("NPC entered trigger zone of the door.");
            npcCount++;
            ToggleDoor(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            Debug.Log("NPC exited trigger zone of the door.");
            npcCount--;
            if (npcCount <= 0)
            {
                npcCount = 0;
                ToggleDoor(false);
            }
        }
    }

}
