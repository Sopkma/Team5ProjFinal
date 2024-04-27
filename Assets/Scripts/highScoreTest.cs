using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighScore;

public class highScoreTest : MonoBehaviour
{
    private float delay;
    // Start is called before the first frame update
    void Start()
    {
        //HighScore.HS.Init(this, "sword attack");
        delay = 1.0f;
    
    }

    // Update is called once per frame
    void Update(){

        if (delay > 0.0f)
        {
            delay -= Time.deltaTime;
            if (delay <= 0.0f)
            {
                //HS.SubmitHighScore(this, "Josh", 0);
            }
        }
    }
}
