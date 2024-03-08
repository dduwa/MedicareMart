using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioSource audioSource;
    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ButtonHandlerPlay()
    {
        coroutine = StartCoroutine(OnButtonPress());
        StartCoroutine(WaitForSoundToFinish(2));
    }

    public IEnumerator OnButtonPress()
    {
        audioSource.Play();
        yield return new WaitForSeconds(2);
    }

    IEnumerator WaitForSoundToFinish(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync("Bus");
        StopCoroutine(coroutine);
    }

    public void ButtonHandlerQuit()
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
 
        SceneManager.LoadSceneAsync("Settings");
    }
}
