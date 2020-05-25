using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : PathfindingAlgorithmBase
{

    private List<Vector2Int> pathFollowed;
    private List<Vector2Int> ShortestPath;

    // Start is called before the first frame update
    void Start()
    {
        pathFollowed = new List<Vector2Int>();
        ShortestPath = new List<Vector2Int>();
    }

    public void DijkstraAlgorithm(Vector2Int start, Vector2Int end, out Dictionary<Vector2Int, Vector2Int> cameFrom)
    {
        Queue<KeyValuePair<Vector2Int, int>> frontier = new Queue<KeyValuePair<Vector2Int, int>>();
        frontier.Enqueue(new KeyValuePair<Vector2Int, int>(start, 0));

        cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int>  costSoFar = new Dictionary<Vector2Int, int>();

        cameFrom.Add(start, start);
        costSoFar.Add(start, 0);

        while (frontier.Count != 0)
        {
            KeyValuePair<Vector2Int, int> current = frontier.Dequeue();
            List<Vector2Int> neighbours = GetNeighbours(current.Key);

            pathFollowed.Add(current.Key);

            if (current.Key == end)
            {
                return;
            }

            foreach (Vector2Int next in neighbours)
            {
                //int new_cost = costSoFar[current.Key] + MapController.instance.map[next.x, next.y] == TileType.Free ? 1 : int.MaxValue; 
                int newCost = costSoFar[current.Key] + 1;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    frontier.Enqueue(new KeyValuePair<Vector2Int, int>(next, newCost));
                    cameFrom[next] = current.Key;
                }
            }
        }
    }

    public override List<Vector2Int> RunAlgorithm(Vector2Int Start, Vector2Int End, out List<Vector2Int> path)
    {
        Dictionary<Vector2Int, Vector2Int> cameFrom;
        DijkstraAlgorithm(Start, End, out cameFrom);

        path = new List<Vector2Int>();

        path.Add(End);
        Vector2Int previousCell = cameFrom[End];
        int stepCounter = 1;
        while (previousCell != Start)
        {
            path.Add(previousCell);
            stepCounter++;

            previousCell = cameFrom[previousCell];
        }
        path.Add(Start);
        path.Reverse();

        return pathFollowed;
    }

}
