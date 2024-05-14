using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    private MusicManager musicManager;
    public GameObject door;
    public GameObject bossHealthUI;
    private bool happened;

    // Start is called before the first frame update
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        happened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !happened)
        {
            happened = true;
            musicManager.StartBossMusic();
            bossHealthUI.SetActive(true);
            BossType1 boss = FindObjectOfType<BossType1>();
            boss.StartBattle();
            door.SetActive(true);
        }
    }
}
