using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuBackground;
    public AudioClip gameBackground;
    public AudioClip playButton;
    public AudioClip buttonClick;
    public AudioClip doorIn;
    public AudioClip storeDoorbell;
    public AudioClip busArrival;
    public AudioClip busDeparture;
    public AudioClip busEngine;
    public AudioClip busHorn;
    public AudioClip carGo;
    public AudioClip npcWalk;
    public AudioClip jumpscare;
    public AudioClip carAlarm;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensures this object persists across scenes
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") // Check if the loaded scene is the menu
        {
            musicSource.clip = menuBackground;
            musicSource.Play();
        }
        else
        {
            musicSource.clip = gameBackground; // Change to another background music or stop music
            musicSource.Play();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the event when the AudioManager is destroyed
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void StopSFX()
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
    }
}
// Code References: https://www.youtube.com/watch?v=N8whM1GjH4w (Unity Audio Manager Tutorial)