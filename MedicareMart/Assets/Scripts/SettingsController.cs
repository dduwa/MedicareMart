using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsController : MonoBehaviour
{
    private AudioSource audioSource;
  
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonHandlerBack()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
