using HighScore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int score;
    public int startMultiplier = 10;
    public int minMultiplier = 1;
    private int multiplier;
    public float timeToDecrament = 20f;
    private float timeSinceLastKill;

    // Start is called before the first frame update
    void Start()
    {
        HighScore.HS.Init(this, "sword attack");
        multiplier = startMultiplier;
        score = 0;
        StartCoroutine(DecramentScore());
        timeSinceLastKill = Time.time - 10;
    }

    // Update is called once per frame
    public void addToScore(int points)
    {
        float calcTime = Time.time - timeSinceLastKill;
        timeSinceLastKill = Time.time;
        print("time: " + calcTime);
        if (calcTime < 0.2 )
        {
            print("MULTIKILL");
            score += (int)(points * multiplier * 1.5f);
        }
        else
        {
            score += points * multiplier;
        }
        print("SCORE: " + score);
    }

    public void addStylePoints(int points)
    {
        score += points * multiplier;
        print("SCORE: " + score);
    }

    public IEnumerator DecramentScore()
    {
        print("Multiplier: " + multiplier);
        while (multiplier > minMultiplier)
        {
            yield return new WaitForSeconds(timeToDecrament);
            multiplier--;
            print("Multiplier: " + multiplier);
        }
    }

    public void OnComplete(string playerName)
    {
        // send to to site;
        HS.SubmitHighScore(this, playerName, score);
    }
}
