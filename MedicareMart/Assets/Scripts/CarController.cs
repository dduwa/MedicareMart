using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
 
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object tagged as "NPC"
        if (collision.gameObject.CompareTag("NPC"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.carGo);
        }
    }
}
