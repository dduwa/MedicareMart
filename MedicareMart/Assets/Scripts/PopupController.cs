using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public GameObject intro;

    // Called when the button is clicked
    public void ShowPopUp()
    {
        // Enable the pop-up panel to show it
        intro.SetActive(true);
    }

    // Called when the close button on the pop-up is clicked
    public void HidePopUp()
    {
        // Disable the pop-up panel to hide it
        intro.SetActive(false);
    }
}
