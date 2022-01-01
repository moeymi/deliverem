using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameSolver
{

    HashSet<string> NodeMap = new HashSet<string>();

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

    public async Task<Stack<GameState>> UCS_Solver(GameState startState)
    {
        return await Task.Run(() =>
        {
            Stack<GameState> path = new Stack<GameState>();

            // Visited Nodes Counter
            double visitedNodes = 0;

            NodeMap.Clear();

            Node startNode = new Node(startState, null);

            NodeMap.Add(startState.GetStringHashcode());

            Node finalNode = null;


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
                    if (!NodeMap.Contains(state.GetStringHashcode()))
                    {
                        Node newNode = new Node(state, currentNode);
                        NodeMap.Add(newNode.GetState().GetStringHashcode());
                        addInOrderPosition(ref priorityQueue, newNode);
                    }
                }
            }
            while (finalNode != null)
            {
                path.Push(finalNode.GetState());
                Debug.LogError(finalNode.GetState().ZuPosition + " ---- " + finalNode.GetState().LastAction);
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
        cost += state.PickedupCoins.Count;
    }
    public GameState GetState() { return state; }
    public Node GetPrevNode() { return prevNode; }
    public int GetCost() { return cost; }
}

