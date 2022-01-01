using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
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
        GameObject.FindGameObjectWithTag("Zu").transform.position =
            new Vector2(state.ZuPosition.y + 0.5f, state.ZuPosition.x + 0.5f);
    }

    static public void AddObject(Vector2Int position, GameObject obj)
    {
        gameObjects.Add(position, obj);
    }

    static public void RemoveCoin(Vector2Int position)
    {
        Destroy(gameObjects[position]);
    }
    static public void Deliver(Vector2Int position)
    {
        gameObjects[position].GetComponent<Placeholder>().Close();
    }
}
