using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockV2 : MonoBehaviour
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

    private bool hasRun = false;


    private State state;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<BoxCollider2D>();
        doorCollision = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
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
                    foreach(Transform child in transform) {
                        child.gameObject.SetActive(true);
                    }
                    hasRun = true;
                } else {
                    break;
                }

                state = State.Fighting;
                break;

            case State.Fighting:
                print("current state is fighting");
                if (!GameObject.FindWithTag("Enemy"))
                {
                    state = State.Finished;
                }
                break;

            case State.Finished:
                print("current state is finished");
                sprite.enabled = false;
                doorCollision.enabled = false;;
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            print("collided");
            state = State.Entered;
        }
    }
}
