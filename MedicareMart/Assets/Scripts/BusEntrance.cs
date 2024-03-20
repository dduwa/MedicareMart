using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusEntrance : MonoBehaviour

{
    public BusMovement busMovement;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Entered");
        if (other.tag == "Player")
        {
            busMovement.PlayerEntered();
// Debug.Log("Player has entered the trigger zone.");


        }
    }
    
}
