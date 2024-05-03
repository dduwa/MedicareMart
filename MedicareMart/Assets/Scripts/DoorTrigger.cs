using UnityEngine;
using UnityEngine.AI;

public class DoorTrigger : MonoBehaviour
{
    public Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) // Make sure your agent has the right tag
        {
            door.ToggleDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            door.ToggleDoor();
        }
    }
}


// Code References: https://github.com/llamacademy/ai-series-part-36/tree/main