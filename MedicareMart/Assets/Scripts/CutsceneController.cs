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

    AudioManager audioManager;

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

        // Find the AudioManager in the scene and get the AudioManager component
        GameObject audioManagerObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
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
        imageToFade.DOFade(0, 2).OnComplete(() => GameManager.Instance.ToggleCrosshair(true));
    }

    IEnumerator CutsceneTwo()
    {
        audioManager.PlaySFX(audioManager.busDeparture);
        Debug.Log("Starting cutscene two, fading to black.");
        ToggleCrosshair(false);
        Tween fadeTween = imageToFade.DOFade(1, 2);
        yield return fadeTween.WaitForCompletion();
        Debug.Log("Fade to black complete, loading next scene.");
        LoadNextScene("Store");
    }


    void ToggleCrosshair(bool isActive)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ToggleCrosshair(isActive);
    }

    void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        StartCoroutine(FadeInOnSceneLoaded());
    }

    IEnumerator FadeInOnSceneLoaded()
    {
        yield return new WaitForEndOfFrame();  // Ensure the scene is loaded
        Image imageInNewScene = GameObject.Find("Image").GetComponent<Image>(); 
        imageInNewScene.color = new Color(0, 0, 0, 1); // Ensure the image is fully black
        imageInNewScene.DOFade(0, 2);  // Fade to transparent
    }

    IEnumerator LoadSceneAfterFade(string sceneName)
    {
        yield return new WaitForSeconds(2); // Wait for the fade to complete
        SceneManager.LoadScene(sceneName);
    }

}
