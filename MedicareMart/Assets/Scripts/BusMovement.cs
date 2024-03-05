using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BusMovement : MonoBehaviour
{
    public Transform[] waypoints; // Assign the waypoints in the inspector
    public float speed = 5f;
    private int waypointIndex = 0;

    private bool isMoving = true;   

    public DoorController doorController;

    // Update is called once per frame
    void Update()
    {
        MoveBus();
    }

    void MoveBus()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            var targetPosition = waypoints[waypointIndex].position;
            var moveDelta = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDelta);

            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                isMoving = true;
            }
            else if (isMoving && waypointIndex == waypoints.Length - 1)
            {
                isMoving = false;
                doorController.OpenDoor();
            }
        }

        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            waypointIndex++;
        }
    }

    // This method can be called by another script, or an event, when the FPS enters the bus
    public void CloseDoor()
    {
        doorController.CloseDoor();
    }
}