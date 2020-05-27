using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyManhatan : PathfindingAlgorithmBase
{
    public void GreedyManhattanAlgorithm(Vector2Int start, Vector2Int end)
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary <Vector2Int,Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom.Add(start, start);

        List<Vector2Int> visited = new List<Vector2Int>();

        while (frontier.Count != 0)
        {
            Vector2Int current = frontier.Dequeue();

            visited.Add(current);
            pathFollowed.Add(current);

            List<Vector2Int> neighbours = GetNeighbours(current);

            Vector2Int cameFromLocal;
            cameFrom.TryGetValue(current, out cameFromLocal);

            neighbours.Remove(cameFromLocal);

            //Remove visited neighbours
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
                FoundPath.Remove(current);
            }

            //Sort neighbours so the element closest to the end (Using Manhattan distance) is first
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

            //If there is a neighbour, check him. Otherwise, backtrack
            if (neighbours.Count > 0)
            {
                Vector2Int nextNeighbour = neighbours[0];
                FoundPath.Add(current);
                if (nextNeighbour == end)
                {
                    pathFollowed.Add(nextNeighbour);
                    return;
                }
                else
                {
                    frontier.Enqueue(nextNeighbour);
                    cameFrom.Add(nextNeighbour, current);
                    visited.Add(current);
                }
            }
            else
            {
                frontier.Enqueue(cameFromLocal);
                FoundPath.Remove(current);
            }

        }
    }

    public override List<Vector2Int> RunAlgorithm(Vector2Int Start, Vector2Int End, out List<Vector2Int> path)
    {
        GreedyManhattanAlgorithm(Start, End);
        path = FoundPath;
        return pathFollowed;
    }
}
