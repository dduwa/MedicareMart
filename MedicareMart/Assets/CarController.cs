using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object tagged as "NPC"
        if (collision.gameObject.CompareTag("NPC"))
        {
            // Play the sound using the AudioManager
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.carGo);
            }
        }
    }
}
