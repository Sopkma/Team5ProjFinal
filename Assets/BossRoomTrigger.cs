using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    private MusicManager musicManager;
    public GameObject door;
    public GameObject bossHealthUI;

    // Start is called before the first frame update
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            musicManager.StartBossMusic();
            bossHealthUI.SetActive(true);
            BossType1 boss = FindObjectOfType<BossType1>();
            boss.StartBattle();
            door.SetActive(true);
        }
    }
}
