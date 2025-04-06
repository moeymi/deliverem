using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes
    static Vector2 startPosition;
    static bool finishedState = true;
    public static Vector2 StartPosition { get => startPosition; set => startPosition = value; }
    public static GameState StartingState { get; private set; }
    #endregion
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public static void CreateFromText(string text)
    {
        StartingState = new GameState(text);
        StartPosition = StartingState.ZuPosition; 
    }
    
    static async void RunFinalPath(Stack<GameState> finalPath)
    {
        while (finalPath.Count > 0)
        {
            if (!finishedState)
            {
                await Task.Delay(720);
                continue;
            }
            GameState nextState = finalPath.Pop();/*
            Debug.Log(nextState.LastAction);
            Debug.Log(nextState.GetStringHashcode());*/
            WorldManager.RunIntoState(nextState);
            finishedState = false;
        }
    }
    static public async void Solve()
    {
        GameSolver solver = new GameSolver();
        float stTime = (Time.realtimeSinceStartup);
        Stack<GameState> path = (await solver.Solve(StartingState));
        Debug.Log("Finished with : " + path.Count + " moves.");
        RunFinalPath(path);
    }

    static public void MakeNextMove()
    {
        finishedState = true;
    }
}
