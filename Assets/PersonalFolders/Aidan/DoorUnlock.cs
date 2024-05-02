using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlock : MonoBehaviour
{

    private enum State {
        Waiting,
        Entered,
        Fighting,
        Finished,
    }

    private CapsuleCollider2D doorCollision;

    private BoxCollider2D triggerCollider;

    private SpriteRenderer sprite;

    public GameObject enemySpawn;
    private SpawnEnemies spawnEnemies;
    //private int enemyNum;
    private int enemyLimit;

    private bool hasRun = false;


    private State state;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        doorCollision = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        spawnEnemies = enemySpawn.GetComponent<SpawnEnemies>();
        spawnEnemies.enabled = false;
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

                // locks door begind player
                sprite.enabled = true;
                doorCollision.enabled = true;

                if (hasRun == false)
                {
                    spawnEnemies.enabled = true;
                    enemyLimit = spawnEnemies.spawnLimit;
                    print(enemyLimit);
                    hasRun = true;
                } else {
                    break;
                }

                state = State.Fighting;
                break;

            case State.Fighting:
                print("current state is fighting");
                int numEnemies = spawnEnemies.totalSpawned;
                print($"spawned enemies is {numEnemies}");

                if (numEnemies != enemyLimit)
                {
                    print("spawning enemies");
                }
                else
                {
                    print("enemies hit limit");
                    if (!GameObject.FindWithTag("Enemy"))
                    {
                        state = State.Finished;
                    }
                }
                break;

            case State.Finished:
                print("current state is finished");
                sprite.enabled = false;
                doorCollision.enabled = false;
                //enemySpawn.SetActive(false);
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            print("collided");
            state = State.Entered;
        }
    }

    //public void EnemiesInRoom () {
    //    if (GameObject.FindWithTag("Enemy")) {
    //        print("Enemies in room");
    //    } else {
    //        state = State.Finished;
    //    }
    //}

}
