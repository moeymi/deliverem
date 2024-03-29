﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class WorldGenerator : MonoBehaviour
{
    #region Attributes
    static Tilemap tileMap;
    static Tile Normal, Obstacle, LeftCorner, RightCorner, LeftEdge, RightEdge, TopEdge;
    static GameObject Coin, Placeholder;
    static Dictionary<int, Color> idColor = new Dictionary<int, Color>();
    static Transform collector;
    #endregion

    private void Awake()
    {
        tileMap = GetComponent<Tilemap>();
        Normal = Resources.Load<Tile>("Tiles/Ground/Normal");
        LeftCorner = Resources.Load<Tile>("Tiles/Ground/LeftCorner");
        RightCorner = Resources.Load<Tile>("Tiles/Ground/RightCorner");
        LeftEdge = Resources.Load<Tile>("Tiles/Ground/LeftEdge");
        RightEdge = Resources.Load<Tile>("Tiles/Ground/RightEdge");
        TopEdge = Resources.Load<Tile>("Tiles/Ground/TopEdge");
        Obstacle = Resources.Load<Tile>("Tiles/Ground/Obstacle");
        Coin = Resources.Load<GameObject>("Prefabs/Coin");

        collector = GameObject.FindGameObjectWithTag("Collector").transform;
        Placeholder = Resources.Load<GameObject>("Prefabs/Placeholder");
    }
    public void Generate(GameState state)
    {
        int n = state.Rows;
        int m = state.Columns;
        GameCell[,] grid = state.GameGrid;
        for (int i = 0;i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                if (grid[i, j].type == GameCellType.Obstacle)
                    tileMap.SetTile(new Vector3Int(j, i, 1), Obstacle);
                else
                    tileMap.SetTile(new Vector3Int(j, i, 1), Normal);
                if(grid[i, j].type == GameCellType.Coin)
                {
                    Color color = new Color();
                    if (idColor.ContainsKey(grid[i, j].id))
                        color = idColor[grid[i, j].id];
                    else
                    {
                        color = Random.ColorHSV(idColor.Keys.Count * 0.15f, idColor.Keys.Count * 0.18f, 0.85f, 0.85f, 1, 1);
                        idColor.Add(grid[i, j].id, color);
                    }
                    GameObject coin = Instantiate(Coin);
                    coin.transform.position = new Vector3(j + 0.5f, i + 0.5f);
                    coin.name = color.ToString();
                    coin.transform.SetParent(collector);
                    coin.GetComponent<SpriteRenderer>().color = color;
                    WorldManager.AddObject(new Vector2Int(j, i), coin);
                }
                else if(grid[i, j].type == GameCellType.Destination)
                {
                    Color color = new Color();
                    if (idColor.ContainsKey(grid[i, j].id))
                        color = idColor[grid[i, j].id];
                    else
                    {
                        color = Random.ColorHSV(idColor.Keys.Count * 0.15f, idColor.Keys.Count * 0.18f, 0.85f, 0.85f, 1, 1);
                        idColor.Add(grid[i, j].id, color);
                    }
                    GameObject placeholder = Instantiate(Placeholder);
                    placeholder.transform.position = new Vector3(j + 0.5f, i + 0.5f);
                    placeholder.name = color.ToString();
                    placeholder.transform.SetParent(collector);
                    placeholder.GetComponent<SpriteRenderer>().color = color;
                    WorldManager.AddObject(new Vector2Int(j, i), placeholder);
                }
            }
        }

        for (int i = 0; i <= n; i++) {
            if(i==n)
                tileMap.SetTile(new Vector3Int(-1, i, 1), LeftCorner);
            else
                tileMap.SetTile(new Vector3Int(-1, i, 1), LeftEdge);
        }
        for (int i = 0; i <= n; i++)
        {
            if (i == n)
                tileMap.SetTile(new Vector3Int(m, i, 1), RightCorner);
            else
                tileMap.SetTile(new Vector3Int(m, i, 1), RightEdge);
        }
        for (int j = 0; j < m; j++)
        {
            tileMap.SetTile(new Vector3Int(j, n, 1), TopEdge);
        }
        Camera.main.transform.position = new Vector3(m / 2f, n / 2f, -10);
        Camera.main.GetComponent<Camera>().orthographicSize = (n+1)/2 + 0.5f;
    }
}
