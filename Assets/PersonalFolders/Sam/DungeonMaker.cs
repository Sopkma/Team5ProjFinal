using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonMaker : MonoBehaviour
{
    void PassOne(){ //this pass makes the layout of the dungeon
        List<Node> nodes = new List<Node>(); // (max 4)
        List<potentialCell> potentialCells = new List<potentialCell>();
        //make start room
        Node tempNode = new Node((0,0));
        nodes.Add(tempNode);
        //connect rooms nsew
        nodes.Add(tempNode.connectNode(Node.direction.north));
        nodes.Add(tempNode.connectNode(Node.direction.east));
        nodes.Add(tempNode.connectNode(Node.direction.south));
        nodes.Add(tempNode.connectNode(Node.direction.west));
        //for each room
        bool finished = false;
        while(!finished){
            foreach (Node node in nodes)
            {
                if (!node.newConnectionMade)
                {
                    if (node.parentCoords != node.relativeCoords(Node.direction.north))
                    {
                        potentialCells.Add(node.reserveCell(Node.direction.north));
                    }
                    //TODO make this work for all directions
                }
            }
        }
        //  check which directions are available

    }
    void PassTwo(){ // this pass puts random cool stuff in 

    }
    
    public List<potentialCell> deleteOverlaps(List<Node> nodes, List<potentialCell> checkList){
        List<potentialCell> result = new List<potentialCell>();
        List<potentialCell> result2 = new List<potentialCell>();
        bool duplicate;
        foreach (potentialCell check in checkList)
        {
            duplicate = false;
            foreach (Node node in nodes)
            {
                if (check.coords == node.coords)
                {
                    duplicate = true;
                }
            }
            if (!duplicate){
                result.Add(check);
            }
        }
        foreach (potentialCell check in result)
        {
            duplicate = false;
            foreach (potentialCell check2 in result)
            {
                if(check2.coords == check.coords && check2 != check){
                    duplicate = true;
                }
            }
            if (!duplicate)
            {
                result2.Add(check);
            }
        }
        return result2;
    }
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
    public enum direction{
        north,
        south,
        east,
        west
    }
    public (int, int) coords; //where the node is on the grid
    public (int, int) parentCoords = (0, 0); //where the parent is
    public List<Node> connectedNodes; // (max 4)
    public bool newConnectionMade = false;
    public Node((int, int) coords){
        this.coords = coords;
    }
    public Node connectNode(direction direction){ //0 is north, 1 is east, 2 is south, 3 is west
        Node tempNode;
        
        if(direction == direction.north){//north
            tempNode = new Node(this.relativeCoords(direction.north));
        } else if(direction == direction.east){//east
            tempNode = new Node(this.relativeCoords(direction.east));
        } else if(direction == direction.south){//south
            tempNode = new Node(this.relativeCoords(direction.south));
        } else {//west
            tempNode = new Node(this.relativeCoords(direction.west));
        }
        tempNode.connectedNodes.Add(this);
        tempNode.parentCoords = this.coords;

        this.connectedNodes.Add(tempNode);
        this.newConnectionMade = true;
        return tempNode;
    }
    public potentialCell reserveCell(direction direction){
        potentialCell tempCell = new potentialCell();
        switch (direction)
        {
            case direction.north:
                tempCell.coords = this.relativeCoords(direction.north);
                break;
            case direction.east:
                tempCell.coords = this.relativeCoords(direction.east);
                break;
            case direction.south:
                tempCell.coords = this.relativeCoords(direction.south);
                break;
            default:
                tempCell.coords = this.relativeCoords(direction.west);
                break;
        }
        tempCell.parent = this;
        return tempCell;
    }

    public bool IsNodeInDirection(direction direction, List<Node> nodes){
        bool answer = false;
        (int, int) desiredCoords = this.relativeCoords(direction);
        foreach (Node node in nodes)
        {
            if (node.coords == desiredCoords)
            {
                answer = true;
            }
        }
        return answer;
    }
    public (int, int) relativeCoords(direction direction){
        switch (direction){
            case direction.north:
                return (this.coords.Item1, this.coords.Item2+1);
            case direction.east:
                return (this.coords.Item1-1, this.coords.Item2);
            case direction.south:
                return (this.coords.Item1, this.coords.Item2-1);
            default:
                return (this.coords.Item1+1, this.coords.Item2);
        }
    }
    
    public List<potentialCell> deleteOverlaps(List<Node> nodes, List<potentialCell> checkList){
        List<potentialCell> result = new List<potentialCell>();
        foreach (Node node in nodes)
        {
            if (node.coords == this.coords)
            {
                //remove potential somehow
            }
        }
        return result;
    }
}

public class potentialCell{
    public (int, int) coords;
    public Node parent;
    public Node becomeNode(){
        Node tempNode = new Node(this.coords);

        //TODO
        return tempNode;
    }
}