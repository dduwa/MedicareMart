using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance { get; private set; }
    public GameObject crosshair;
    public Image imageToFade;

    public Camera busCam;

    void Start()
    {
        if (imageToFade != null)
        {
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0); // Set alpha to 0
        }
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
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
        Debug.Log("Starting cutscene two, fading to black.");
        ToggleCrosshair(false);
        Tween fadeTween = imageToFade.DOFade(1, 2);
        yield return fadeTween.WaitForCompletion();
        Debug.Log("Fade to black complete, loading next scene.");
        LoadNextScene("Store");
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
        StartCoroutine(LoadSceneAfterFade(sceneName));
    }

    IEnumerator LoadSceneAfterFade(string sceneName)
    {
        yield return new WaitForSeconds(2); // Wait for the fade to complete
        SceneManager.LoadScene(sceneName);
    }

}
