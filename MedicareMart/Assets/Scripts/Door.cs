using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Door : MonoBehaviour
{
    private NavMeshObstacle obstacle;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 90, 0)); // Adjust rotation axis as needed
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        obstacle.carving = !isOpen;

        if (isOpen)
            StartCoroutine(AnimateDoor(openRotation));
        else
            StartCoroutine(AnimateDoor(closedRotation));
    }

    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, time);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}

// Code References: https://github.com/llamacademy/ai-series-part-36/tree/main