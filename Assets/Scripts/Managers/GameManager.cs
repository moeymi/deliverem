using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes
    private static Vector2 startPosition;
    GameState startingState;
    static bool finishedState = true;
    public static Vector2 StartPosition { get => startPosition; set => startPosition = value; }
    #endregion
    private void Awake()
    {
        startingState = new GameState("Assets/Readables/state1.txt");
        StartPosition = startingState.ZuPosition; 
        Debug.Log(startingState.GetStringHashcode());
        WorldManager.GenerateWorld(startingState);
    }
    async void RunFinalPath(Stack<GameState> finalPath)
    {
        while (finalPath.Count > 0)
        {
            if (!finishedState)
            {
                await Task.Delay(1000);
                continue;
            }
            WorldManager.RunIntoState(finalPath.Pop());
            finishedState = false;
        }
    }

    private void Start()
    {
        Solve();
    }

    async void Solve()
    {
        GameSolver solver = new GameSolver();
        float stTime = (Time.realtimeSinceStartup);
        Stack<GameState> path = (await solver.UCS_Solver(startingState));
        Debug.LogWarning(path.Count);
        Debug.LogWarning("FINISH");
        Debug.LogError("Time = " + (Time.realtimeSinceStartup - stTime));
        RunFinalPath(path);
    }

    static public void MakeNextMove()
    {
        finishedState = true;
    }
}
