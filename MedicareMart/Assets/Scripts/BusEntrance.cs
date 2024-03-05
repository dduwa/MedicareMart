using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusEntrance : MonoBehaviour

{
    public BusMovement busMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            busMovement.CloseDoor();
        }
    }
    
}
