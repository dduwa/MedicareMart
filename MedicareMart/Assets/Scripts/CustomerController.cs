using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    private Animator animator;
    private bool isTalking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.Play("Idle");  // Ensures that "Idle" is the name of the idle state
    }

    public void StartTalking()
    {
        isTalking = true;
        Debug.Log("Customer is talking");
    }

    public void EndTalkingAndMove()
    {
        isTalking = false;
        Debug.Log("Customer is done talking");

        // Optionally, start a cutscene and then deactivate the customer
        if (CutsceneController.Instance != null)
        {
            CutsceneController.Instance.StartCutscene(3);  // Assuming '3' is a valid cutscene ID
            DeactivateCustomer();
        }
        else
        {
            Debug.LogError("CutsceneController instance not found!");
        }
    }

    private void DeactivateCustomer()
    {
        Debug.Log("Deactivating customer.");
        gameObject.SetActive(false);  // Deactivates the customer GameObject
    }
}
