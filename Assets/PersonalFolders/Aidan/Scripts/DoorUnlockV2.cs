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

    /* put all door child objects in this array in inspector for each opening in the room, the doors do not require enemies under them just simple doors like the ones in the prefab
     * this will allow all the doors triggers to work for whichever way you enter a room
     */
    public BoxCollider2D[] doors;

    private CapsuleCollider2D[] doorCollision;

    private BoxCollider2D[] triggerCollider;

    private SpriteRenderer[] sprite;

    private bool hasRun = false;


    private State state;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponentsInChildren<BoxCollider2D>();
        doorCollision = GetComponentsInChildren<CapsuleCollider2D>();
        sprite = GetComponentsInChildren<SpriteRenderer>();
        foreach (var doorSprite in sprite)
        {
            doorSprite.enabled = false;
        }
        
        foreach (var door in doorCollision)
        {
            door.enabled = false;
        }

        foreach (var doorCollider in doors)
            doorCollider.gameObject.AddComponent<DoorColliderHandler>().Init(this);

        state = State.Waiting;
    }

    private void Update() {
        switch (state) {
            case State.Waiting:
                break;

            case State.Entered:
                print("current state is entered");

                // deactivate trigger collider for doors
                foreach (var trigger in triggerCollider)
                {
                    trigger.enabled = false;
                }
                

                // locks door behind player
                foreach (var doorSprite in sprite)
                {
                    doorSprite.enabled = true;
                }
                
                foreach (var door in doorCollision)
                {
                    door.enabled = true;
                }
                   
                // activates each of the enemies in the children of the doors
                if (hasRun == false)
                {
                    foreach(Transform child in transform) {
                        foreach (Transform child2 in child)
                        {
                            child2.gameObject.SetActive(true);
                        }
                        
                    }
                    hasRun = true;
                } else {
                    break;
                }

                state = State.Fighting;
                break;

            case State.Fighting:
                print("current state is fighting");

                /* state WILL NOT CHANGE until all objects with enemy tags are deleted
                 * (gonna try to make this associated with the children objects rather than ALL objects with enemy tag)
                */
                if (!GameObject.FindWithTag("Enemy"))
                {
                    state = State.Finished;
                }
                break;

            case State.Finished:
                //print("current state is finished");

                // disables the door colliders and sprites to allow player to go through doorways again
                foreach (var doorSprite in sprite)
                {
                    doorSprite.enabled = false;
                }

                foreach (var door in doorCollision)
                {
                    door.enabled = false;
                }
                break;
        }
    }

    public void HandleDoorTriggerExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            state = State.Entered;
            Debug.Log("collided");
        }
    }
}

public class DoorColliderHandler : MonoBehaviour
{
    private DoorUnlockV2 doorUnlockV2;

    public void Init(DoorUnlockV2 doorUnlockV2)
    {
        this.doorUnlockV2 = doorUnlockV2;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        doorUnlockV2.HandleDoorTriggerExit(other);
    }
}
