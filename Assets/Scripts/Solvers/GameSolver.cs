using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DataStructures;
using TMPro;

public class GameSolver
{
    #region Attributes
    HashSet<string> NodeMap = new HashSet<string>();
    static bool isSolving = false;
    #endregion
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

    async Task<Stack<GameState>> RunSolver(GameState startState, int heuristic)
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


            PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>(1000);
            priorityQueue.Insert(startNode, startNode.GetCost());

            while (priorityQueue.Size() != 0)
            {
                visitedNodes++;

                Node currentNode = priorityQueue.Pop();

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
                        //Debug.Log(state.Heuristic_2());
                        Node newNode = new Node(state, currentNode);
                        NodeMap.Add(newNode.GetState().GetStringHashcode());
                        if(heuristic == 1)
                            priorityQueue.Insert(newNode, newNode.GetCost() + state.Heuristic_1());
                        else if (heuristic == 2)
                            priorityQueue.Insert(newNode, newNode.GetCost() + state.Heuristic_2());
                        else if (heuristic == 3)
                            priorityQueue.Insert(newNode, newNode.GetCost() + state.Heuristic_3());
                        else
                            priorityQueue.Insert(newNode, newNode.GetCost());
                    }
                }
            }
            while (finalNode != null)
            {
                path.Push(finalNode.GetState());
                //Debug.LogError(finalNode.GetState().ZuPosition + " ---- " + finalNode.GetState().LastAction);
                finalNode = finalNode.GetPrevNode();
            }
            Debug.Log("Visited Nodes = " + visitedNodes);

            return path;
        });
    }

    public async Task<Stack<GameState>> Solve(GameState startGamestate)
    {
        int heuristic = UIManager.DropdownValue;
        isSolving = true;
        Stack<GameState> finalPath = (await RunSolver(startGamestate, heuristic));
        isSolving = false;
        return finalPath;
    }

    static public bool IsSolving
    {
        get { return isSolving; }
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
            cost += prevNode.cost;
        cost += state.PickedupCoins.Count;
    }
    public GameState GetState() { return state; }
    public Node GetPrevNode() { return prevNode; }
    public int GetCost() { return cost; }
}

