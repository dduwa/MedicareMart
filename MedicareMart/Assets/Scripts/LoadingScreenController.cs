using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public Text progressText;


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }
    private IEnumerator LoadAsynchronously(string sceneName)
    {
        Debug.Log($"Starting to load scene: {sceneName}");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);

        while (operation.progress < 0.9f)
        {
            // Progress reaches 0.9 when it's ready to activate, but waits for your command
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            progressText.text = (int)(progress * 100f) + "%";
            Debug.Log($"Loading progress: {progressText.text}");
            yield return null;
        }

        // Do any setup before activating the scene
        Debug.Log("Ready to activate scene.");
        operation.allowSceneActivation = true; // Activate the scene

        while (!operation.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene loaded successfully.");
        loadingScreen.SetActive(false);
    }



}
