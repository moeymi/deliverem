using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Solver
{
    Dictionary<string, Node> NodeMap = new Dictionary<string, Node>();

    public async Task<Stack<Node>> UCS_Solver (GameState startState)
    {
        return await Task.Run(() =>
        {
            Stack<Node> path = new Stack<Node>();

            // Visited Nodes Counter
            double visitedNodes = 0;

            NodeMap.Clear();

            Node startNode = new Node(startState, null); 

            NodeMap.Add(startState.GetStringHashcode(), startNode);

            Node finalNode = null;

            Node prevNode = null;

            List<Node> priorityQueue = new List<Node>
            {
                startNode
            };

            while (priorityQueue.Count != 0)
            {
                visitedNodes++;

                Node currentNode = priorityQueue[0];
                priorityQueue.RemoveAt(0);

                if (GameManager.isFinalState(currentNode.getState()))
                {
                    finalNode = currentNode;
                    break;
                }

                List<GameState> nextStates = currentNode.getState().GetNextStates(); 

                foreach (var state in nextStates)
                {
                    if (!NodeMap.ContainsKey(state.GetStringHashcode()))
                    {
                        Node newNode = new Node(state, prevNode);
                        NodeMap.Add(newNode.getState().GetStringHashcode(), newNode);
                        priorityQueue.Add(newNode); 
                    }
                }          
            }
            while(finalNode != null)
            {
                path.Push(finalNode);
                finalNode = finalNode.getPrevNode(); 
            }

            Debug.LogWarning("Visited Nodes = " + visitedNodes); 

            return path;
        }); 
}
public class Node
{
    GameState state;
    Node prevNode;
    int cost = 1;

    public Node(GameState state, Node prevNode)
    {
        this.state = state;
        this.prevNode = prevNode;
        if (prevNode != null)
            this.cost += prevNode.cost;
    }
    public GameState getState() { return state; }
    public Node getPrevNode() { return prevNode; }
    public int getCost() { return cost; }
}
