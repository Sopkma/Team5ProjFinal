using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Helper : MonoBehaviour
{
    public Game game;
    public AudioClip deathSound;
    public GameObject player;
    private AudioSource audioSource;
    public GameObject musicManager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Help(){
        Debug.Log("helping");
        game.GameOver();
    }
    public void PlayDeathSound(){
        musicManager.GetComponent<MusicManager>().DeathStop();
        audioSource.PlayOneShot(deathSound);
    }
}
