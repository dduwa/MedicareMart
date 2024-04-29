using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    AudioManager audioManager; // Reference to the AudioManager
    private Coroutine coroutine;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonHandlerPlay()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.playButton); // Play button sound
        }

        StartCoroutine(WaitForSoundToFinishAndLoadScene("Bus", audioManager.playButton.length));
    }


    IEnumerator WaitForSoundToFinishAndLoadScene(string sceneName, float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
        // Load the scene.
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void ButtonHandlerQuit()
    {
        if (audioManager != null)
        {
            // Play the button click sound.
            audioManager.PlaySFX(audioManager.buttonClick);

            // Start a coroutine to quit after the sound has finished playing.
            StartCoroutine(WaitForSoundToFinishAndQuit(audioManager.buttonClick.length));
        }
        else
        {
            // If AudioManager is not found, quit immediately.
            QuitApplication();
        }
    }

    IEnumerator WaitForSoundToFinishAndQuit(float delay)
    {
        // Wait for the length of the clip to ensure it has finished playing.
        yield return new WaitForSeconds(delay);
        // Quit the application.
        QuitApplication();
    }

    private void QuitApplication()
    {
        // If we are running in a standalone build of the game
#if UNITY_STANDALONE
    // Quit the application
    Application.Quit();
#endif

        // If we are running in the editor
#if UNITY_EDITOR
    // Stop playing the scene
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    public void ButtonHandlerSettings()
    {
        //Debug.Log("Settings button pressed");
        if (audioManager != null)
        {
           //Debug.Log("AudioManager found, playing click sound");
            audioManager.PlaySFX(audioManager.buttonClick); // Play button click sound
        }

        StartCoroutine(WaitForSoundToFinishAndLoadScene("Settings", audioManager.buttonClick.length));
    }
}
