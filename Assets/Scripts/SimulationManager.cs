using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimulationManager : MonoBehaviour
{
    //Public
    public GameObject GoButton;
    public GameObject runnerPrefab;
    public GameObject highlightPrefab;
    public GameObject GameCompletedPopup;

    //Private
    private PathfindingAlgorithmBase[] algorithms;
    private List<List<Vector2Int>> results;
    private List<List<Vector2Int>> shortestPaths;
    private List<float> timeSpent;

    // Start is called before the first frame update
    void Start()
    {
        //All algorithms are kept as Components
        algorithms = GetComponents<PathfindingAlgorithmBase>();
        timeSpent = new List<float>();
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

            //Counting execution time of each algorithm
            float timeAtBegining = Time.realtimeSinceStartup;

            List<Vector2Int> result = algorithm.RunAlgorithm(Start, End, out List<Vector2Int> path);

            timeSpent.Add(Time.realtimeSinceStartup - timeAtBegining);

            shortestPaths.Add(path);
            results.Add(result);
        }
        //Whole simulation is done in a routine, to let the player observe while his input is not blocked
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
            //Instantiate the runners
            GameObject runner = Instantiate(runnerPrefab, MapController.instance.grid.transform);
            runner.transform.position = MapController.CellToWorld(MapController.instance.StartX, MapController.instance.StartY);

            //Change the size and sorting order so you can see them all (smallest one is rendered on top)
            runner.transform.localScale = new Vector3(scale - (scaleIncrement * results.IndexOf(result)), scale - (scaleIncrement * results.IndexOf(result)), 1f);
            SpriteRenderer runnerRenderer = runner.GetComponent<SpriteRenderer>();
            runnerRenderer.color = algorithms[results.IndexOf(result)].runnerColor;
            runnerRenderer.sortingOrder = results.IndexOf(result);

            runners.Add(runner);

            //Initializing maximum length of the simulation
            if(result.Count > maxLength)
            {
                maxLength = result.Count;
            }
        }

        yield return new WaitForSeconds(0.2f);

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
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.3f);

        foreach (List<Vector2Int> path in shortestPaths)
        {
            foreach(Vector2Int CellLocation in path)
            {
                GameObject runner = Instantiate(runners[shortestPaths.IndexOf(path)], MapController.instance.grid.transform);
                runner.transform.position = MapController.CellToWorld(CellLocation.x, CellLocation.y);
            }
        }

        yield return new WaitForSeconds(1f);

        //Show game completed popup
        GameCompletedPopup.SetActive(true);


        //Record the results
        List<string> algNames = new List<string>();
        List<int> numOfFields = new List<int>();

        foreach (PathfindingAlgorithmBase algorithm in algorithms)
        {
            algNames.Add(algorithm.algorithmName);
        }
        foreach (List<Vector2Int> result in results)
        {
            numOfFields.Add(result.Count);
        }
        Debug.Log(OptionsController.instance.MapSize);

        RunResult runResult = new RunResult(OptionsController.instance.MapSize, OptionsController.instance.NumberOfObstacles, algNames, numOfFields, timeSpent);
        ResultsHolder.runResults.Add(runResult);
    }
}

