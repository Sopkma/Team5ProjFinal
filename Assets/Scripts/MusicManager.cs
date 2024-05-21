using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicState
{
    START,
    NORMAL,
    BATTLE,
    BOSS
}

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private MusicState musicState;
    [Header("First song is the normal, non battle music\nSecond is the boss intro\nThird is the boss song loop\nForth is the normal battle music\n5th is the death music")]
    public AudioClip[] songs;
    private AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicState = MusicState.START;
        PlayOutsideBattle();
        // StartBossMusic();
    }

    public void PlayOutsideBattle()
    {
        if (musicState != MusicState.NORMAL)
        {
            musicSource.Stop();
            musicState = MusicState.NORMAL;
            musicSource.clip = songs[0];
            musicSource.Play();
        }
    }

    public void PlayBattleMusic()
    {
        if (musicState != MusicState.BATTLE)
        {
            musicSource.Stop();
            musicState = MusicState.BATTLE;
            musicSource.clip = songs[3];
            musicSource.Play();
        }
    }

    public void StartBossMusic()
    {
        if (musicState != MusicState.BOSS)
        {
            musicState = MusicState.BOSS;
            musicSource.Stop();
            musicSource.clip = songs[1];
            StartCoroutine(BossIntro());
        }
    }

    public void DeathMusic(){
        musicSource.clip = songs[4];
        musicSource.Play();
    }
    public void DeathStop(){
        musicSource.Stop();
    }

    private IEnumerator BossIntro()
    {
        musicSource.Play();
        yield return new WaitForSecondsRealtime(musicSource.clip.length);
        if (musicState.Equals(MusicState.BOSS))
        {
            musicSource.clip = songs[2];
            musicSource.Play();
        }
        
    }
}
