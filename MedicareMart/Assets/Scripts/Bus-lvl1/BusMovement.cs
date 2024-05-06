using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BusMovement : MonoBehaviour
{
    public Transform[] waypoints; // Assign the waypoints in the inspector
    public float speed = 5f;
    private int waypointIndex = 0;
    public BusDoorController doorController;
    public BusDriverController busDriverController;
    
    
    private bool playerEntered = false; // This should be set to true by another script when the player enters a specific trigger

    public Camera busCam; // Reference to the BusCam camera


    void Start()
    {
        UIManager.Instance.TriggerObjective("Take the bus to work.");

        BeginCutsceneAfterDelay(2); // Start the first cutscene

         // Parent the BusCam to this bus GameObject
        busCam.transform.SetParent(transform, false);
        busCam.transform.localPosition = Vector3.zero; // Set the local position to zero to keep the camera at the pivot point of the bus
        busCam.transform.localRotation = Quaternion.identity; // Optionally, reset the local rotation if needed
    }

    IEnumerator BeginCutsceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CutsceneController.Instance.StartCutscene(1); 
    }
    void Update()
    {
        MoveBus();
    }

      public void PlayerEntered()
    {
        busDriverController.IdleSitting(); // Set the bus driver to idle sitting
       
        Debug.Log("PlayerEntered method called"); // Ensure this is called

        playerEntered = true;
    }

    void MoveBus()
    {
        if (waypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[waypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                if (waypointIndex == 0) // Correctly check for the first waypoint
                {
                    busDriverController.Driving(); // Set the bus driver to driving
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.busArrival); // Sound for bus starting
                }

                if (waypointIndex == waypoints.Length - 1)
                {
                    doorController.OpenDoor(); // Open the door at the last waypoint

                    if (playerEntered)
                    {
                        CloseDoor();
                    }
                }
                else
                {
                    waypointIndex++;
                }
            }
        }
    }

    public void CloseDoor()
    {
        doorController.CloseDoor();
    }
}