using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for loading scenes

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance { get; private set; }
    public GameObject crosshair;
    public Image imageToFade;

    public Camera busCam;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional, based on your needs
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public method to start a specific cutscene
    public void StartCutscene(int cutsceneId)
    {
        switch (cutsceneId)
        {
            case 1:
                StartCoroutine(CutsceneOne());
                break;
            case 2:
                StartCoroutine(CutsceneTwo());
                break;
            default:
                Debug.LogWarning("Cutscene ID not recognized.");
                break;
        }
    }

    IEnumerator CutsceneOne()
    {
        // Cutscene for starting the game: Fade from black and toggle crosshair
        yield return new WaitForSeconds(1);
        imageToFade.DOFade(0, 2).OnComplete(() => ToggleCrosshair(true));
    }

    IEnumerator CutsceneTwo()
    {
        // Disable the crosshair at the beginning of the cutscene
        ToggleCrosshair(false);

        // Now fade to black
        Tween fadeTween = imageToFade.DOFade(1, 2); // Adjust the duration as needed
        yield return fadeTween.WaitForCompletion();

        // After fading to black, load the next scene
        LoadNextScene("Store"); // Replace "Store" with your actual next scene name
    }

    void ToggleCrosshair(bool isActive)
    {
        if (crosshair != null)
        {
            crosshair.SetActive(isActive);
        }
    }

    void LoadNextScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
