﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DataStructures;
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


            DataStructures.PriorityQueue<Node, int> priorityQueue = new DataStructures.PriorityQueue<Node, int>(1000);
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
                        priorityQueue.Insert(newNode, newNode.GetCost()+state.Heuristic_1());
                    }
                }
            }
            while (finalNode != null)
            {
                path.Push(finalNode.GetState());
                //Debug.LogError(finalNode.GetState().ZuPosition + " ---- " + finalNode.GetState().LastAction);
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
            cost += prevNode.cost;
        cost += state.PickedupCoins.Count;
    }
    public GameState GetState() { return state; }
    public Node GetPrevNode() { return prevNode; }
    public int GetCost() { return cost; }
}

