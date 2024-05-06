using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save();  // Make sure to save PlayerPrefs to disk
        Debug.Log("Score saved: " + score);
    }


    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score updated: " + score);
        SaveScore();
    }

    public void LoadScore()
    {
        score = PlayerPrefs.GetInt("PlayerScore", 0);
        Debug.Log("Score loaded: " + score);
   
    }

    public int GetScore()
    {
        return score;
    }


    public void ResetScore()
    {
        score = 0;
        PlayerPrefs.DeleteKey("PlayerScore");
        PlayerPrefs.Save();
        Debug.Log("Score reset and PlayerPrefs cleared");
    }
}

