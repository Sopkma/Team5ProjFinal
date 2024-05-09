using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Node{
    public (int, int) coords; //where the node is on the grid
    public List<(int, int)> exits; // (north, south, east, west)
    public List<String> todos; // add key, locked doors, puzzles, solutions
    public void fillExit((int, int) exitCoords, List<Node> nodeList){
        bool funkyRoom = false;
        if (!funkyRoom) // if the room is normal:
        {
            foreach (Node node in nodeList)
            {
                if (node.exits.Contains(exitCoords)){
                    funkyRoom=false;
                }
            }
        }
    }
}