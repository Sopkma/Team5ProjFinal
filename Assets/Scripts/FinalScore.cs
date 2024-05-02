using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScore : MonoBehaviour
{
    public ScoreManager scoreManager;
    public TextMeshProUGUI scoreTxt;
    public string playerName;
    public TextMeshProUGUI nameTxt;

    // Start is called before the first frame update
    void Awake()
    {
        print("SCORE: " + scoreManager.GetScore());
    }

    public void ChangeName(TextMeshProUGUI name)
    {
        playerName = name.text;
    }

    public void SubmitScore()
    {
        scoreManager.SendScore(nameTxt.text);
        SceneManager.LoadScene(0);
    }
}
