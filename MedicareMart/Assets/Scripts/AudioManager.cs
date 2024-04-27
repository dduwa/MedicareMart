using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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



    // Start is called before the first frame update
    void Start()
    {
        // Check if there's a GameObject with the "Menu" tag present in the scene
        if (GameObject.FindGameObjectWithTag("Menu") != null)
        {
            // If a Menu object is found, set the menu background music and play it
            musicSource.clip = menuBackground;
            musicSource.Play();
        }
        else
        {
            // Optionally, handle the situation if no Menu object is found
            // For example, you might want to play a different music track or do nothing
            // musicSource.clip = someOtherBackgroundMusic;
            // musicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

}
// Code References: https://www.youtube.com/watch?v=N8whM1GjH4w (Unity Audio Manager Tutorial)