using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsController : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        // Find the AudioManager in the scene and get the AudioManager component
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
    }

    IEnumerator WaitForSoundToFinishAndLoadScene(string sceneName, float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
        // Load the scene.
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ButtonHandlerBack()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonClick); // Play button click sound
        }

        StartCoroutine(WaitForSoundToFinishAndLoadScene("Menu", audioManager.buttonClick.length));
    }
}
