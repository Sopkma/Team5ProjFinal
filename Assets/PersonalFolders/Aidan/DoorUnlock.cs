using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{

    private enum State {
        Waiting,
        Entered,
        Finished,
    }

    private CapsuleCollider2D doorCollision;

    private BoxCollider2D triggerCollider;

    private BoxCollider2D roomCollider;

    private SpriteRenderer sprite;

    public GameObject enemySpawn;
    private SpawnEnemies spawnEnemies;
    private int enemyNum;


    private State state;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        doorCollision = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        roomCollider = GetComponentInChildren<BoxCollider2D>();

        enemySpawn.SetActive(false);
        sprite.enabled = false;
        doorCollision.enabled = false;
        
        state = State.Waiting;
    }

    private void Update() {
        switch (state) {
            case State.Waiting:
                break;

            case State.Entered:
                print("current state is entered");
                triggerCollider.enabled = false;
                sprite.enabled = true;
                doorCollision.enabled = true;
                EnemiesInRoom(roomCollider);
                enemySpawn.SetActive(true);
                spawnEnemies = GetComponentInChildren<SpawnEnemies>();
                enemyNum = spawnEnemies.spawnLimit;
                break;

            case State.Finished:
                print("current state is finished");
                sprite.enabled = false;
                doorCollision.enabled = false;
                enemySpawn.SetActive(false);
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            print("collided");
            state = State.Entered;
        }
    }

    public void EnemiesInRoom (BoxCollider2D roomCollider) {
        if (enemyNum > 0 ) {
            print("Enemies in room");
        } else {
            state = State.Finished;
        }
    }

}
