using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene());
    }

    
    public Image imageToFade;

    IEnumerator Cutscene(){
        yield return new WaitForSeconds(1);
        imageToFade.DOFade(0, 2);

    }
 
}
