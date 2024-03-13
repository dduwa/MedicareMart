using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public GameObject crosshair;
    public Image imageToFade;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene());
    }

 

    IEnumerator Cutscene(){
        yield return new WaitForSeconds(1);
        imageToFade.DOFade(0, 2).OnComplete(() => ToggleCrosshair(true));
    

    }

    void ToggleCrosshair(bool isActive){
            if(crosshair != null) // Check if the crosshair reference is set
        {
            crosshair.SetActive(isActive);
        };
    }
 
}
