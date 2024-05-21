using HighScore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ScoreManager : MonoBehaviour
{
    int score;
    public int startMultiplier = 10;
    public int minMultiplier = 1;
    private int multiplier;
    public float timeToDecrament = 20f;
    private float timeSinceLastKill;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI scoreMultiplierTxt;
    public GameObject txtPrefab;
    public Player player;
    private AudioSource audioSource;
    public AudioClip coinAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        HighScore.HS.Init(this, "blade diver");
        multiplier = startMultiplier;
        //score = 0;
        score = PlayerPrefs.GetInt("score");
        scoreTxt.text = "" + score;
        StartCoroutine(DecramentScore());
        timeSinceLastKill = Time.time - 10;
    }

    public void addToScore(int points)
    {
        float calcTime = Time.time - timeSinceLastKill;
        timeSinceLastKill = Time.time;
        print("time: " + calcTime);
        if (calcTime < 0.2 )
        {
            print("MULTIKILL");
            Instantiate(txtPrefab, parent: player.transform);
            score += (int)(points * multiplier * 1.5f);
        }
        else
        {
            score += points * multiplier;
        }
        // print("SCORE: " + score);
        scoreTxt.text = "" + score;
    }

    public void addStylePoints(int points)
    {
        score += points * multiplier;
        // print("SCORE: " + score);
        scoreTxt.text = "" + score;
    }

    public IEnumerator DecramentScore()
    {
        // print("Multiplier: " + multiplier);
        scoreMultiplierTxt.text = multiplier + "x";
        while (multiplier > minMultiplier)
        {
            yield return new WaitForSeconds(timeToDecrament);
            multiplier--;
            scoreMultiplierTxt.text = multiplier + "x";
        }
    }

    public void StoreScore()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }

    public void SendScore(string playerName)
    {
        // HS.Clear(this);
        // send highscore to site
        HS.SubmitHighScore(this, playerName, score);
    }

    public void CallCountCoins()
    {
        StartCoroutine(CountCoin());
    }

    public int GetScore()
    {
        return score;
    }

    private IEnumerator CountCoin()
    {
        player.speed = 0;
        while(player.coinCounter > 0)
        {
            score += 10;
            player.coinCounter--;
            player.coinsTxt.text = "Coins: " + player.coinCounter;
            player.coinsUITxt.text = "" + player.coinCounter;
            audioSource.PlayOneShot(coinAudioClip);
            scoreTxt.text = "" + score;

            yield return new WaitForSeconds(0.1f);
        }

        Game gameManager = FindAnyObjectByType<Game>();
        gameManager.EndGame();
    }
}
