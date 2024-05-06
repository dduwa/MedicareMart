using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager instance is null.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (AudioManager.Instance.carAlarm != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.carAlarm);  
                Debug.Log("Player entered trigger: Car alarm sound played.");
                StartCoroutine(StopSoundAfterDelay(3));  // Stop the sound after 3 seconds
            }
            else
            {
                Debug.LogError("Car alarm sound clip is missing.");
            }
        }
        else if (other.CompareTag("NPC"))
        {
            // Check if the carGo clip is available
            if (AudioManager.Instance.carGo != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.carGo);
                StartCoroutine(DisableCarAfterDelay(3));
            }
        }
    }

    private IEnumerator StopSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  

        // Stop the sound
        AudioManager.Instance.StopSFX();
        Debug.Log("Car alarm sound stopped after delay.");
    }

    private IEnumerator DisableCarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        Debug.Log("Car GameObject has been disabled after delay.");
    }

}
