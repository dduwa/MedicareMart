using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance { get; private set; }
    public GameObject crosshair;
    public Camera busCam;
    public Image ImageToFade { get; private set; }

    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure the singleton doesn't get destroyed on load
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        SetupImageToFade();
    }

    private void SetupImageToFade()
    {
        GameObject imageObject = GameObject.FindGameObjectWithTag("ImageToFade");
        if (imageObject != null)
        {
            ImageToFade = imageObject.GetComponent<Image>();
            if (ImageToFade != null)
            {
                // Set initial state of the image, for example, fully transparent
                ImageToFade.color = new Color(ImageToFade.color.r, ImageToFade.color.g, ImageToFade.color.b, 0);
            }
        }
        else
        {
            Debug.LogWarning("No object with 'ImageToFade' tag found. Check your scene setup.");
        }
    }


    private void FadeImage(Image image, float targetAlpha, float duration, Action onComplete = null)
    {
        if (image != null && image.gameObject.activeInHierarchy)
        {
            image.DOFade(targetAlpha, duration).OnComplete(() => onComplete?.Invoke());
        }
        else
        {
            Debug.LogWarning("Attempted to fade a null or inactive image.");
        }
    }



    void OnDestroy()
    {
        if (ImageToFade != null)
        {
            ImageToFade.DOKill(); // Stop any DOTween animations on this image
        }
    }

    public void OnSceneUnload()
    {
        DOTween.KillAll(); 
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
            case 3:
                StartCoroutine(CutsceneThree());
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
        FadeImage(ImageToFade, 0, 2, () => UIManager.Instance.ToggleCrosshair(true));
    }

    IEnumerator CutsceneTwo()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.busDeparture);
        Debug.Log("Starting cutscene two, fading to black.");
        UIManager.Instance.ToggleCrosshair(false);
        yield return new WaitForSeconds(2); // Allow SFX to play and not overlap with fading
        FadeImage(ImageToFade, 1, 2, () =>
        {
            Debug.Log("Fade to black complete, loading next scene.");
            LoadNextScene("Store");
        });
    }

    IEnumerator CutsceneThree()
    {
        Debug.Log("Starting cutscene three, fading to black.");
       // UIManager.Instance.ToggleCrosshair(false);
        yield return new WaitForSeconds(2); // Allow SFX to play and not overlap with fading
        FadeImage(ImageToFade, 1, 2, () =>
        {
            StartCoroutine(FadeInOnSceneLoaded());
            Debug.Log("Fade to black complete, loading next scene.");
        });
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
        if (imageInNewScene != null)
        {
            UIManager.Instance.ToggleCrosshair(false);
            imageInNewScene.color = new Color(0, 0, 0, 1); // Ensure the image is fully black
            FadeImage(imageInNewScene, 0, 2);  // Fade to transparent
        }
        else
        {
            Debug.LogWarning("No image found to fade in on new scene.");
        }
    }

}
