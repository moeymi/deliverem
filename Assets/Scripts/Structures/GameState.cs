using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameState
{
    #region Attributes
    GameCell[,] gameGrid;
    List<int> pickedUpCoins = new List<int>();
    int n, m;
    Vector2Int zuPosition;
    Action lastAction = Action.Move;
    #endregion
    public GameState(GameState state)
    {
        n = state.n;
        m = state.m;
        this.gameGrid = new GameCell[n,m];
        for(int i = 0; i < n; i++)
        {
            for(int j = 0;j < m; j++)
            {
                gameGrid[i, j] = new GameCell(state.GameGrid[i, j]);
            }
        }
        this.pickedUpCoins = new List<int>(state.pickedUpCoins);
        zuPosition = new Vector2Int(state.ZuPosition.x, state.ZuPosition.y);
        lastAction = state.lastAction;
    }

    public GameState(string fileContent)
    {
        using (StringReader reader = new StringReader(fileContent))
        {
            // Use Regex.Split to split on any whitespace (tabs, spaces, etc.)
            string line = reader.ReadLine();
            string[] nums = Regex.Split(line.Trim(), @"\s+");

            n = int.Parse(nums[0]);
            m = int.Parse(nums[1]);
            gameGrid = new GameCell[n, m];

            List<string> lines = new List<string>();
            for (int i = 0; i < n; i++)
            {
                lines.Add(reader.ReadLine());
            }
            lines.Reverse();
            for (int i = 0; i < n; i++)
            {
                line = lines[i];
                // Again use Regex.Split for flexible splitting
                string[] cells = Regex.Split(line.Trim(), @"\s+");
                for (int j = 0; j < m; j++)
                {
                    if (cells[j] == "#")
                        SetObstacle(new Vector2Int(i, j));
                    else if (cells[j][0] == 'P')
                    {
                        int id = int.Parse(cells[j].Substring(1));
                        SetCoin(new Vector2Int(i, j), id);
                    }
                    else if (cells[j][0] == 'D')
                    {
                        int id = int.Parse(cells[j].Substring(1));
                        SetDestination(new Vector2Int(i, j), id);
                    }
                    else if (cells[j] == "T")
                    {
                        zuPosition = new Vector2Int(i, j);
                        SetEmpty(new Vector2Int(i, j));
                    }
                    else
                        SetEmpty(new Vector2Int(i, j));
                }
            }
        }
    }

    public List<GameState> GetNextStates()
    {
        List<GameState> gameStates = new List<GameState>();

        List<GameState> leftStates = GetNextStateForOneAction(new Vector2Int(zuPosition.x - 1, zuPosition.y));
        List<GameState> rightStates = GetNextStateForOneAction(new Vector2Int(zuPosition.x + 1, zuPosition.y));
        List<GameState> topStates = GetNextStateForOneAction(new Vector2Int(zuPosition.x, zuPosition.y +1));
        List<GameState> bottomStates = GetNextStateForOneAction(new Vector2Int(zuPosition.x, zuPosition.y - 1));

        if (leftStates != null&& leftStates.Count != 0 )
        {
            gameStates.AddRange(leftStates);
        }

        if (rightStates != null && rightStates.Count != 0)
        {
            gameStates.AddRange(rightStates);
        }

        if (topStates != null && topStates.Count != 0)
        {
            gameStates.AddRange(topStates);
        }

        if (bottomStates != null && bottomStates.Count != 0)
        {
            gameStates.AddRange(bottomStates);
        }

        return gameStates;
    }
    List<GameState> GetNextStateForOneAction(Vector2Int newPosition)
    {
        if (!checkValidPosition(newPosition))
            return null; 

        List<GameState> gameStates = new List<GameState>();

        //Update Zu position 
        GameState currentState = new GameState(this);
        currentState.SetAction(Action.Move);
        currentState.SetZuPosition(newPosition);

        // If Zu on coin's destination that he has 
        if (GameGrid[newPosition.x, newPosition.y].type == GameCellType.Destination)
        {
            if(pickedUpCoins.Contains(GameGrid[newPosition.x, newPosition.y].id))
            {
                int i = pickedUpCoins.IndexOf(GameGrid[newPosition.x, newPosition.y].id);
                GameState newState = new GameState(currentState);
                newState.SetFinishedDestination(newPosition);

                List<int> newPickedCoins = newState.PickedupCoins;
                newPickedCoins.RemoveAt(i);
                newState.PickedupCoins = newPickedCoins;

                newState.SetAction(Action.Deliver);
                gameStates.Add(newState);
                return gameStates;
            }
        }
        //Just skip the cell situation
        gameStates.Add(currentState);
        //If the current cell has coin 
        if (GameGrid[newPosition.x, newPosition.y].type == GameCellType.Coin)
        {
            GameState newState = new GameState(currentState);
            List<int> newPickedCoins = newState.PickedupCoins;
            newPickedCoins.Add(newState.GameGrid[newPosition.x, newPosition.y].id);
            newState.PickedupCoins = newPickedCoins;

            newState.SetEmpty(newPosition);

            newState.SetAction(Action.Pickup);

            gameStates.Add(newState);
        }



        return gameStates;
    }

    bool checkValidPosition(Vector2Int position)
    {
        if (position.x < n && position.x >= 0 && position.y < m && position.y >= 0 &&
            GameGrid[position.x, position.y].type != GameCellType.Obstacle)
            return true;
        return false;
    }
    public void SetEmpty(Vector2Int position)
    {
        gameGrid[position.x, position.y] = new GameCell();
    }
    public void SetObstacle(Vector2Int position)
    {
        gameGrid[position.x, position.y] = new GameCell(GameCellType.Obstacle);
    }
    public void SetCoin(Vector2Int position, int id)
    {
        gameGrid[position.x, position.y] = new GameCell(GameCellType.Coin, id);
    }
    public void SetDestination(Vector2Int position, int id)
    {
        gameGrid[position.x, position.y] = new GameCell(GameCellType.Destination, id);
    }
    public void SetFinishedDestination(Vector2Int position)
    {
        gameGrid[position.x, position.y].type = GameCellType.FinishedDestination;
    }
    public void SetZuPosition(Vector2Int position)
    {
        zuPosition = position;
    }
    public void SetAction(Action action)
    {
        this.lastAction = action;
    }

    public string GetStringHashcode()
    {
        string str = "";

        //Cells
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                GameCell curCell = gameGrid[i, j];
                str += curCell.type.ToString()[0];
                str += curCell.id.ToString();
            }
        }

        str += "|";
        //PickedUps
        foreach (int id in pickedUpCoins)
            str += id.ToString();
        str += "|";
        //Position
        str += zuPosition.x.ToString();
        str += zuPosition.y.ToString();

        return str;
    }
    public int Rows
    {
        get { return n; }
    }
    public int Columns
    {
        get { return m; }
    }
    public GameCell[,] GameGrid
    {
        get { return gameGrid; }
    }
    public Vector2Int ZuPosition
    {
        get { return zuPosition; }
    }
    public List<int> PickedupCoins
    {
        get { return pickedUpCoins; }
        set { pickedUpCoins = value; }
    }
    public Action LastAction
    {
        get { return lastAction; }
    }
    public bool isFinalState()
    {
        if (zuPosition != GameManager.StartPosition)
            return false;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (gameGrid[i, j].type == GameCellType.Coin || gameGrid[i, j].type == GameCellType.Destination)
                    return false;
            }
        }

        return true;
    }

    public int  Heuristic_1()
    {
        int x_diff = (int)Mathf.Abs(GameManager.StartPosition.x - zuPosition.x);
        int y_diff = (int)Mathf.Abs(GameManager.StartPosition.y - zuPosition.y);

        return x_diff + y_diff;
    }
    public List<int> GetAllCoinsCost(bool isRandom)
    {
        Dictionary<int, Vector2Int> remainingCoins = new Dictionary<int, Vector2Int>();
        Dictionary<int, Vector2Int> remainingDestinations = new Dictionary<int, Vector2Int>();
        List<int> allCosts = new List<int>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (gameGrid[i, j].type == GameCellType.Coin)
                {
                    remainingCoins[gameGrid[i, j].id] = new Vector2Int(i, j);
                }
                else if (gameGrid[i, j].type == GameCellType.Destination)
                {
                    remainingDestinations[gameGrid[i, j].id] = new Vector2Int(i, j);
                }
                if (isRandom)
                {
                    int id = gameGrid[i, j].id;
                    if (remainingDestinations.ContainsKey(gameGrid[i, j].id) 
                        && remainingCoins.ContainsKey(gameGrid[i, j].id))
                    {
                        int distance_x = (int)Mathf.Abs(remainingDestinations[id].x - remainingCoins[id].x);
                        int distance_y = (int)Mathf.Abs(remainingDestinations[id].x - remainingCoins[id].y);
                        int total_distance = distance_x + distance_y;
                        allCosts.Add(total_distance);
                    }
                }
            }
        }
        //Calculate all distancess
        foreach (var id in remainingCoins.Keys)
        {
            int distance_x = Mathf.Abs(remainingDestinations[id].x - remainingCoins[id].x);
            int distance_y = Mathf.Abs(remainingDestinations[id].x - remainingCoins[id].y);
            int total_distance = distance_x + distance_y;
            allCosts.Add(total_distance);
        }

        //Check the picked up coins 
        for (int i = 0; i < pickedUpCoins.Count; i++)
        {
            int coin_Id = pickedUpCoins[i];
            int distance_x = Mathf.Abs(remainingDestinations[coin_Id].x - zuPosition.x);
            int distance_y = Mathf.Abs(remainingDestinations[coin_Id].y - zuPosition.y);
            int total_distance = distance_x + distance_y;
            allCosts.Add(total_distance);
            if (isRandom)
                return allCosts;

        }
        return allCosts;
    }
    public int Heuristic_2()
    {
        List<int> allCosts = GetAllCoinsCost(true);
        if (allCosts.Count > 0)
            return allCosts[0];
        //if all coins are deliverd 
        else
            return 0;
    }
    public int Heuristic_3()
    {
        List<int> allCosts = GetAllCoinsCost(false);
        int max = 0;
        for (int i = 0; i < allCosts.Count; i++)
            max = Mathf.Max(max, allCosts[i]);

        return max;
    }

}
