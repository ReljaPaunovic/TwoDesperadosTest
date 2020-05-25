using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyManhatan : PathfindingAlgorithmBase
{
    private List<Vector2Int> pathFollowed;
    private List<Vector2Int> ShortestPath;

    // Start is called before the first frame update
    void Start()
    {
        pathFollowed = new List<Vector2Int>();
        ShortestPath = new List<Vector2Int>();
    }

    public void GreedyManhattanAlgorithm(Vector2Int start, Vector2Int end, out Dictionary<Vector2Int, Vector2Int> cameFrom)
    {
        

        Queue<KeyValuePair<Vector2Int, int>> frontier = new Queue<KeyValuePair<Vector2Int, int>>();
        frontier.Enqueue(new KeyValuePair<Vector2Int, int>(start, 0));

        Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
        costSoFar.Add(start, 0);

        cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom.Add(start, start);

        List<Vector2Int> visited = new List<Vector2Int>();

        while (frontier.Count != 0)
        {
            KeyValuePair<Vector2Int, int> current = frontier.Dequeue();

            visited.Add(current.Key);
            pathFollowed.Add(current.Key);

            List<Vector2Int> neighbours = GetNeighbours(current.Key);

            Vector2Int cameFromLocal;
            cameFrom.TryGetValue(current.Key, out cameFromLocal);

            neighbours.Remove(cameFromLocal);

            List<Vector2Int> neighboursToRemove = new List<Vector2Int>();
            foreach(Vector2Int neighbour in neighbours)
            {
                if(visited.Contains(neighbour))
                {
                    neighboursToRemove.Add(neighbour);
                }
            }
            foreach(Vector2Int neighbour in neighboursToRemove)
            {
                neighbours.Remove(neighbour);
            }

            neighbours.Sort(delegate (Vector2Int a, Vector2Int b)
            {
                int aManhattan = PathfinderHelper.ManhattanDistance(a.x, a.y, end.x, end.y);
                int bManhattan = PathfinderHelper.ManhattanDistance(b.x, b.y, end.x, end.y);

                if (aManhattan > bManhattan)
                {
                    return 1;
                }
                if (aManhattan < bManhattan)
                {
                    return -1;
                }
                return 0;
            });


            if (neighbours.Count > 0)
            {
                Vector2Int nextNeighbour = neighbours[0];

                if (nextNeighbour == end)
                {
                    pathFollowed.Add(nextNeighbour);
                    return;
                }
                else
                {
                    frontier.Enqueue(new KeyValuePair<Vector2Int, int>(nextNeighbour, current.Value + 1));
                    cameFrom.Add(nextNeighbour, current.Key);
                    visited.Add(current.Key);
                }
            }
            else
            {
                frontier.Enqueue(new KeyValuePair<Vector2Int, int>(cameFromLocal, current.Value - 1));
            }

        }
    }

    public override List<Vector2Int> RunAlgorithm(Vector2Int Start, Vector2Int End, out List<Vector2Int> path)
    {
        Dictionary<Vector2Int, Vector2Int> cameFrom;
        GreedyManhattanAlgorithm(Start, End, out cameFrom);

        path = new List<Vector2Int>();
        /*
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
        path.Reverse();*/

        return pathFollowed;
    }
}
