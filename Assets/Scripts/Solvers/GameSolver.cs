using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameSolver
{
    Dictionary<string, Node> NodeMap = new Dictionary<string, Node>();

    public void addInOrderPosition(ref List<Node> pQueue, Node newNode)
    {
        pQueue.Add(newNode);
        if (pQueue.Count > 1)
            for (int i = pQueue.Count - 1; i > 0; i--)
            {
                if (pQueue[i].GetState().GetHashCode() < pQueue[i - 1].GetState().GetHashCode())
                {
                    Node t = pQueue[i];
                    pQueue[i] = pQueue[i - 1];
                    pQueue[i - 1] = t;
                }
            }
    }

    public async Task<Stack<Node>> UCS_Solver(GameState startState)
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

                if (currentNode.GetState().isFinalState())
                {
                    finalNode = currentNode;
                    break;
                }

                List<GameState> nextStates = currentNode.GetState().GetNextStates();
                

                foreach (var state in nextStates)
                {
                    if (!NodeMap.ContainsKey(state.GetStringHashcode()))
                    {
                        Node newNode = new Node(state, currentNode);
                        NodeMap.Add(newNode.GetState().GetStringHashcode(), newNode);
                        addInOrderPosition(ref priorityQueue, newNode);
                    }
                }
            }
            while (finalNode != null)
            {
                path.Push(finalNode);
                finalNode = finalNode.GetPrevNode();
            }
            Debug.LogWarning("Visited Nodes = " + visitedNodes);

            return path;
        });
    }
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
    public GameState GetState() { return state; }
    public Node GetPrevNode() { return prevNode; }
    public int GetCost() { return cost; }
}

