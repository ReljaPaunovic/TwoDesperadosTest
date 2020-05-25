using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public GameObject GoButton;
    public GameObject runnerPrefab;
    public GameObject highlightPrefab;
    private PathfindingAlgorithmBase[] algorithms;

    public List<List<Vector2Int>> results;
    public List<List<Vector2Int>> shortestPaths;

    // Start is called before the first frame update
    void Start()
    {
        algorithms = GetComponents<PathfindingAlgorithmBase>();

        results = new List<List<Vector2Int>>();
        shortestPaths = new List<List<Vector2Int>>();
    }

    public void RunSimulation()
    {
        GoButton.SetActive(false);

        foreach(PathfindingAlgorithmBase algorithm in algorithms)
        {
            Vector2Int Start = new Vector2Int(MapController.instance.StartX, MapController.instance.StartY);
            Vector2Int End = new Vector2Int(MapController.instance.EndX, MapController.instance.EndY);

            List<Vector2Int> path;
            List<Vector2Int> result = algorithm.RunAlgorithm(Start, End, out path);

            shortestPaths.Add(path);
            results.Add(result);
        }

        StartCoroutine(SimulationRoutine());
    }

    private IEnumerator SimulationRoutine()
    {
        List<GameObject> runners = new List<GameObject>();
        int maxLength = -1;
        float scale = 3;
        float scaleIncrement = 1f / results.Count * 2;

        foreach (List<Vector2Int> result in results)
        {
            GameObject runner = Instantiate(runnerPrefab, MapController.instance.grid.transform);
            runner.transform.position = MapController.CellToWorld(MapController.instance.StartX, MapController.instance.StartY);

            //Randomize the size so you can see them all
            runner.transform.localScale = new Vector3(scale - (scaleIncrement * results.IndexOf(result)), scale - (scaleIncrement * results.IndexOf(result)), 1f);
            SpriteRenderer runnerRenderer = runner.GetComponent<SpriteRenderer>();
            runnerRenderer.color = algorithms[results.IndexOf(result)].runnerColor;
            runnerRenderer.sortingOrder = results.IndexOf(result);

            runners.Add(runner);
            if(result.Count > maxLength)
            {
                maxLength = result.Count;
            }
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < maxLength; i++)
        {
            foreach (List<Vector2Int> result in results)
            {
                if(i < result.Count)
                {
                    runners[results.IndexOf(result)].transform.position = MapController.CellToWorld(result[i].x, result[i].y);

                    GameObject highlightGO = Instantiate(highlightPrefab, MapController.instance.grid.transform);
                    highlightGO.transform.position = MapController.CellToWorld(result[i].x, result[i].y);
                    
                    Color highlightColor = algorithms[results.IndexOf(result)].runnerColor;
                    highlightColor.a = 0.5f;
                    SpriteRenderer highlightGoRenderer = highlightGO.GetComponent<SpriteRenderer>();
                    highlightGoRenderer.color = highlightColor;
                    highlightGoRenderer.sortingOrder = -1;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (List<Vector2Int> path in shortestPaths)
        {
            foreach(Vector2Int CellLocation in path)
            {
                GameObject runner = Instantiate(runners[shortestPaths.IndexOf(path)], MapController.instance.grid.transform);
                runner.transform.position = MapController.CellToWorld(CellLocation.x, CellLocation.y);
            }
        }
       
    }
}

