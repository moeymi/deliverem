using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldManager
{
    #region Attributes
    static WorldGenerator worldGenerator;
    static Dictionary<Vector2Int, GameObject> gameObjects = new Dictionary<Vector2Int, GameObject>();

    #endregion

    static public void GenerateWorld(GameState state)
    {
        if (worldGenerator == null)
            worldGenerator = GameObject.FindGameObjectWithTag("Grid").GetComponent<WorldGenerator>();
        worldGenerator.Generate(state);
    }

    static public void AddObject(Vector2Int position, GameObject obj)
    {
        gameObjects.Add(position, obj);
    }
}
