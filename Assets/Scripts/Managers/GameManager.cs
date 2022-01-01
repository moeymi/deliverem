using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes
    private static Vector2 startPosition;
    public static Vector2 StartPosition { get => startPosition; set => startPosition = value; }
    #endregion
    async private void Awake()
    {
        GameState startingState = new GameState("Assets/Readables/state1.txt");
        StartPosition = startingState.ZuPosition; 
        Debug.Log(startingState.GetStringHashcode());
        WorldManager.GenerateWorld(startingState);
        GameSolver solver = new GameSolver();
        float stTime = (Time.realtimeSinceStartup);
        Debug.LogWarning((await solver.UCS_Solver(startingState)).Count);
        Debug.LogWarning("FINISH");
        Debug.LogError("Time = " + (Time.realtimeSinceStartup - stTime));
    }
    void RunFinalPath(Stack<GameState> finalPath)
    {

    }
}
