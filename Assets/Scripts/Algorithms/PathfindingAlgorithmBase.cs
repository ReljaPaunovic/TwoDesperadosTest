using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfindingAlgorithmBase : MonoBehaviour
{
    //Assigned through inspector (name to be shown to player)
    public string algorithmName;
    //Assigned through inspector
    public Color runnerColor;

    //Neighbours
    public Vector2Int[] directions = { new Vector2Int(1, 0),
                                       new Vector2Int(-1, 0),
                                       new Vector2Int(0, 1),
                                       new Vector2Int(0, -1),
    };

    //Path the the algorithm was taking
    protected List<Vector2Int> pathFollowed;

    //The path that the algorithm outputs
    protected List<Vector2Int> FoundPath;

    void Start()
    {
        pathFollowed = new List<Vector2Int>();
        FoundPath = new List<Vector2Int>();
    }

    public List<Vector2Int> GetNeighbours(Vector2Int origin)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        int MapSizeLocal = OptionsController.instance.MapSize;

        //Remove neighbours that are blocked (no reason to check them)
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbour = origin + direction;

            if ((neighbour.x < MapSizeLocal && neighbour.x >= 0) && (neighbour.y < MapSizeLocal && neighbour.y >= 0) && MapController.instance.map[neighbour.x, neighbour.y] != TileType.Blocked)
                neighbours.Add(origin + direction);
        }
        return neighbours;
    }

    public abstract List<Vector2Int> RunAlgorithm(Vector2Int Start, Vector2Int End, out List<Vector2Int> path);
}
