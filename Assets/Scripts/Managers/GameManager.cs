using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes
    static Vector2 startPosition; 
    #endregion
    private void Awake()
    {
        GameState startingState = new GameState("Assets/Readables/state1.txt");
        Debug.Log(startingState.GetStringHashcode());
        WorldManager.GenerateWorld(startingState);
    }
    void RunFinalPath(Stack<GameState> finalPath)
    {

    }
    static public bool isFinalState(GameState state)
    {
        if (state.ZuPosition != GameManager.startPosition)
            return false;
        
        for (int i =0; i < state.Rows; i++)
        {
            for (int j =0; j<state.Columns; j++)
            {
                if (state.GameGrid[i, j].type == GameCellType.Coin || state.GameGrid[i, j].type == GameCellType.Destination)
                    return false; 
            }
        }

        return true; 
    }

}
