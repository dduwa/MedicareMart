using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndingManager : MonoBehaviour
{
    public Text endingText;

    private void Awake()
    {

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ToggleCursorVisibility(true);
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.playButton);
        UpdateEndingText();
      
    }

    void UpdateEndingText()
    {
        int finalScore = ScoreManager.Instance.GetScore();
        string message = DetermineEndingMessage(finalScore);
        endingText.text = message;
    }

    string DetermineEndingMessage(int score)
    {
        if (score == 0)
            return "Ending 1/3 - Went home because still sick.";
        else if (score > 50)
            return "Ending 2/3 - You died to the killer. ";
        else
            return "Ending 3/3 - You survived the killer. ";
    }

    public void ButtonHandlerQuit()
    {
        // Play the button click sound.
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        GameManager.Instance.QuitGame();

    }

    public void ButtonHandlerRestart()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        GameManager.Instance.RestartGame();
    }
}
