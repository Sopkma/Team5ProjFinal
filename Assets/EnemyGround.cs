using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    public DoorUnlockV2 door;
    private bool entered;

    // Start is called before the first frame update
    void Start()
    {
        entered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("ENTERED");
        if (collision.gameObject.CompareTag("Player") && !entered)
        {
            print("ENTERED2");
            entered = true;
            door.StartBattle();
        }
    }
}
